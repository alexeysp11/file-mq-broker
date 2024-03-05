using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileMqBroker.MqLibrary.DAL;
using FileMqBroker.MqLibrary.DirectoryOperations;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.QueueDispatchers;

/// <summary>
/// Class for processing the message queue from the consumer side.
/// </summary>
public class ConsumerMessageQueueDispatcher : IMessageQueueDispatcher
{
    private readonly string m_requestDirectoryName;
    private readonly string m_responseDirectoryName;
    private readonly int m_oneTimeProcQueueElements;
    private readonly MessageFileDAL m_messageFileDAL;
    private readonly ExceptionDAL exceptionDAL;
    private readonly FileHandler m_fileHandler;
    private readonly MessageFileQueue m_messageFileQueue;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ConsumerMessageQueueDispatcher(
        AppInitConfigs appInitConfigs, 
        MessageFileDAL messageFileDAL,
        ExceptionDAL exceptionDAL,
        FileHandler fileHandler, 
        MessageFileQueue messageFileQueue)
    {
        m_oneTimeProcQueueElements = appInitConfigs.OneTimeProcQueueElements;
        m_requestDirectoryName = appInitConfigs.RequestDirectoryName;
        m_responseDirectoryName = appInitConfigs.ResponseDirectoryName;
        m_messageFileDAL = messageFileDAL;
        m_fileHandler = fileHandler;
        m_messageFileQueue = messageFileQueue;
    }

    /// <summary>
    /// Method for processing the message queue from the consumer side.
    /// </summary>
    public void ProcessMessageQueue()
    {
        var fileMessages = m_messageFileDAL.GetMessageFileInfo(null, 20_000, 1);
        ThreadPool.QueueUserWorkItem(state =>
        {
            foreach (var fileMessage in fileMessages)
            {
                fileMessage.MessageFileState = MessageFileState.Reading;
                m_messageFileQueue.EnqueueMessage(fileMessage);
            }
        });
        var processingTasks = new Task[fileMessages.Count];
        for (int i = 0; i < fileMessages.Count; i++)
        {
            Task task = Task.Run(() =>
            {
                ProcessFileReadRequest(fileMessages[i]);
            });
            processingTasks[i] = task;
        }
        Task.WaitAll(processingTasks);
        
        // 
        var logggingMessages = m_messageFileQueue.DequeueMessagesLogging(m_oneTimeProcQueueElements);
        ThreadPool.QueueUserWorkItem(state =>
        {
            m_messageFileDAL.UpdateMessageFileState(logggingMessages);
        });

        // 
        var exceptions = m_messageFileQueue.DequeueExceptionLogging(m_oneTimeProcQueueElements);
        ThreadPool.QueueUserWorkItem(state =>
        {
            exceptionDAL.InsertExceptions(exceptions);
        });
    }

    /// <summary>
    /// Performs processing of the specified file (create the file in the directory).
    /// </summary>
    private void ProcessFileReadRequest(MessageFile fileMessage)
    {
        var fileName = fileMessage.Name;
        var fileContent = fileMessage.Content;
        try
        {
            fileMessage.Content = m_fileHandler.ReadFromFile(fileName);
            m_fileHandler.DeleteFile(fileName);
            fileMessage.MessageFileState = MessageFileState.Processed;
        }
        catch (Exception ex)
        {
            fileMessage.MessageFileState = MessageFileState.FailedToRead;
            m_messageFileQueue.EnqueueExceptionLogging($"Error processing file {fileName}: {ex.Message}");
        }
        finally
        {
            m_messageFileQueue.EnqueueMessageLogging(fileMessage);
        }
    }
}
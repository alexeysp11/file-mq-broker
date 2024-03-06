using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileMqBroker.MqLibrary.DAL;
using FileMqBroker.MqLibrary.DirectoryOperations;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.QueueDispatchers;

/// <summary>
/// Class for processing the message queue as a writer.
/// </summary>
public class WriteMqDispatcher : IMqDispatcher
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
    public WriteMqDispatcher(
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
    /// Method for processing the message queue from the producer side.
    /// </summary>
    public void ProcessMessageQueue()
    {
        // 
        var fileMessages = m_messageFileQueue.DequeueMessages(m_oneTimeProcQueueElements);
        ThreadPool.QueueUserWorkItem(state =>
        {
            foreach (var fileMessage in fileMessages)
            {
                fileMessage.MessageFileState = MessageFileState.Writing;
            }
            m_messageFileDAL.InsertMessageFileState(fileMessages);
        });
        var processingTasks = new Task[fileMessages.Count];
        for (int i = 0; i < fileMessages.Count; i++)
        {
            Task task = Task.Run(() =>
            {
                ProcessFileCreateRequest(fileMessages[i]);
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
    private void ProcessFileCreateRequest(MessageFile fileMessage)
    {
        var fileName = fileMessage.Name;
        var fileContent = fileMessage.Content;
        try
        {
            m_fileHandler.CreateFile(fileName);
            m_fileHandler.WriteToFile(fileName, fileContent);
            fileMessage.MessageFileState = MessageFileState.ReadyToRead;
        }
        catch (Exception ex)
        {
            fileMessage.MessageFileState = MessageFileState.FailedToWrite;
            m_messageFileQueue.EnqueueExceptionLogging($"Error processing file {fileName}: {ex.Message}");
        }
        finally
        {
            m_messageFileQueue.EnqueueMessageLogging(fileMessage);
        }
    }
}
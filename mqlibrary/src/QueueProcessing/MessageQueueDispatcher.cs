using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileMqBroker.MqLibrary.DAL;
using FileMqBroker.MqLibrary.DirectoryOperations;
using FileMqBroker.MqLibrary.Models;
using FileMqBroker.MqLibrary.RuntimeQueues;

namespace FileMqBroker.MqLibrary.QueueProcessing;

/// <summary>
/// Performs a key mediator role within message queue processing.
/// </summary>
public class MessageQueueDispatcher : IProducerMessageQueueDispatcher, IConsumerMessageQueueDispatcher
{
    private readonly string m_requestDirectoryName;
    private readonly string m_responseDirectoryName;
    private readonly MessageFileDAL m_messageFileDAL;
    private readonly FileHandler m_fileHandler;
    private readonly MessageFileQueue m_messageFileQueue;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MessageQueueDispatcher(
        AppInitConfigs appInitConfigs, 
        MessageFileDAL messageFileDAL, 
        FileHandler fileHandler, 
        MessageFileQueue messageFileQueue)
    {
        m_requestDirectoryName = appInitConfigs.RequestDirectoryName;
        m_responseDirectoryName = appInitConfigs.ResponseDirectoryName;
        m_messageFileDAL = messageFileDAL;
        m_fileHandler = fileHandler;
        m_messageFileQueue = messageFileQueue;
    }

    /// <summary>
    /// Method for processing the message queue from the producer side.
    /// </summary>
    public void ProcessProducer()
    {
        var fileMessages = m_messageFileQueue.DequeueMessages(10000);

        ThreadPool.QueueUserWorkItem(state =>
        {
            m_messageFileDAL.UpdateMessageFileState(fileMessages, MessageFileState.Reading);
        });

        foreach (var fileMessage in fileMessages)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                ProcessFileCreateRequest(fileMessage);
            });
        }
    }

    /// <summary>
    /// Method for processing the message queue from the consumer side.
    /// </summary>
    public void ProcessConsumer()
    {
        // 
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
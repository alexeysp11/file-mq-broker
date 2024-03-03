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
    private readonly MessageFileDAL m_messageFileDAL;
    private readonly FileHandler m_fileHandler;
    private readonly MessageFileQueue m_messageFileQueue;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MessageQueueDispatcher(MessageFileDAL messageFileDAL, FileHandler fileHandler, MessageFileQueue messageFileQueue)
    {
        m_messageFileDAL = messageFileDAL;
        m_fileHandler = fileHandler;
        m_messageFileQueue = messageFileQueue;
    }

    /// <summary>
    /// Method for processing the message queue from the producer side.
    /// </summary>
    public void ProcessProducer()
    {
        List<string> fileNames = m_messageFileQueue.DequeueMessages(10000); 

        ThreadPool.QueueUserWorkItem(state =>
        {
            m_messageFileDAL.UpdateMessageFileState(fileNames, MessageFileState.Reading);
        });

        foreach (string fileName in fileNames)
        {
            ThreadPool.QueueUserWorkItem(state =>
            {
                ProcessFileCreate(fileName);
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
    private void ProcessFileCreate(string fileName)
    {
        try
        {
            m_fileHandler.CreateFile(fileName);
            m_fileHandler.WriteToFile(fileName, "Sample data");

            Console.WriteLine($"File {fileName} processed successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing file {fileName}: {ex.Message}");
        }
    }
}
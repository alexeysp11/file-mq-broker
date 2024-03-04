using System.Collections.Generic;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.RuntimeQueues;

/// <summary>
/// An interface that allows you to manage a message queue within an application instance.
/// </summary>
public interface IMessageFileQueue
{
    /// <summary>
    /// Enqueues a message into the message queue.
    /// </summary>
    void EnqueueMessage(MessageFile message);

    /// <summary>
    /// Dequeues a specified number of messages from the message queue.
    /// </summary>
    List<MessageFile> DequeueMessages(int count);

    /// <summary>
    /// Enqueues a message into the logging queue.
    /// </summary>
    void EnqueueMessageLogging(MessageFile message);

    /// <summary>
    /// Dequeues a specified number of messages from the logging queue.
    /// </summary>
    List<MessageFile> DequeueMessagesLogging(int count);

    /// <summary>
    /// Enqueues an exception message into the exception logging queue.
    /// </summary>
    void EnqueueExceptionLogging(string exceptionMessage);

    /// <summary>
    /// Dequeues a specified number of exception messages from the exception logging queue.
    /// </summary>
    List<string> DequeueExceptionLogging(int count);
}
namespace FileMqBroker.MqLibrary.QueueProcessing;

/// <summary>
/// An interface that allows you to implement a message queue manager on the consumer side.
/// </summary>
public interface IConsumerMessageQueueDispatcher
{
    /// <summary>
    /// Method for processing the message queue from the consumer side.
    /// </summary>
    void ProcessConsumer();
}
namespace FileMqBroker.MqLibrary.QueueProcessing;

/// <summary>
/// An interface that allows you to implement a message queue manager on the producer side.
/// </summary>
public interface IProducerMessageQueueDispatcher
{
    /// <summary>
    /// Method for processing the message queue from the producer side.
    /// </summary>
    void ProcessProducer();
}
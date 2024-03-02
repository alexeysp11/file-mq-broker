namespace FileMqBroker.MqLibrary.QueueProcessing;

/// <summary>
/// 
/// </summary>
public class MessageQueueDispatcher
{
    // This class could be described as follows:
    // - perform the function of a mediator: communicates directly in compliance with established levels:
    //       - a class located at the DAL level (performs queries to the database),
    //       - a class for working with directories,
    //       - a class that contains a message queue for processing in memory.
    // - runs on a timer.
    // - plays a very important role in the application.
}
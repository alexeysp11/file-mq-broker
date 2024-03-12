using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.ResponseHandlers;

/// <summary>
/// 
/// </summary>
public interface IResponseHandler
{
    /// <summary>
    /// Method that is used to process a message received from a message broker.
    /// </summary>
    void ContinuationMethod(MessageFile messageFile);
}
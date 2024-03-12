using FileMqBroker.MqLibrary.DAL;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.ResponseHandlers;

/// <summary>
/// 
/// </summary>
public class SqliteResponseHandler : IResponseHandler
{
    private MessageFileResponseDAL m_responseDAL;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public SqliteResponseHandler(MessageFileResponseDAL responseDAL)
    {
        m_responseDAL = responseDAL;
    }

    /// <summary>
    /// Method that is used to process a message received from a message broker.
    /// </summary>
    public void ContinuationMethod(MessageFile messageFile)
    {
        m_responseDAL.InsertMessageFileResponse(messageFile);
    }
}
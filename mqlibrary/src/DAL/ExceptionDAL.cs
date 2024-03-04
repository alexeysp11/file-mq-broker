namespace FileMqBroker.MqLibrary.DAL;

/// <summary>
/// 
/// </summary>
public class ExceptionDAL
{
    private readonly string m_connectionString;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ExceptionDAL(string connectionString)
    {
        m_connectionString = connectionString;
    }
}
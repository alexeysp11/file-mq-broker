using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.DAL;

/// <summary>
/// 
/// </summary>
public class MessageFileResponseDAL
{
    private readonly string m_connectionString;
    private readonly string m_defaultInsertResponseSQL = "INSERT INTO Responses (Name, Content) VALUES (@response_Name, @response_Content)";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MessageFileResponseDAL(AppInitConfigs appInitConfigs)
    {
        m_connectionString = appInitConfigs.DbConnectionString;
    }

    /// <summary>
    /// Method for inserting the specified response into DB.
    /// </summary>
    public void InsertMessageFileResponse(MessageFile messageFile)
    {
        if (messageFile == null)
            throw new System.ArgumentNullException(nameof(messageFile));

        var responseContent = $"The file '{messageFile.Name}' is processed";
        
        var parameters = new DynamicParameters();
        parameters.Add($"response_Name", messageFile.Name);
        parameters.Add($"response_Content", responseContent);

        using (var connection = new SQLiteConnection(m_connectionString))
        {
            connection.Execute(m_defaultInsertResponseSQL, parameters);
        }
    }
}
using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.DAL;

/// <summary>
/// Defines the functionality required for communication with the database at the Data Access Layer (DAL) level 
/// as part of working with a message.
/// </summary>
public class MessageFileDAL
{
    private readonly string m_connectionString;
    private readonly string m_defaultSelectAllSQL = "SELECT m.Name, m.MessageFileState FROM MessageFiles m";

    /// <summary>
    /// Default constructor.
    /// </summary>
    public MessageFileDAL(string connectionString)
    {
        m_connectionString = connectionString;
    }

    /// <summary>
    /// Method for obtaining information about files.
    /// </summary>
    public List<MessageFile> GetMessageFileInfo(IReadOnlyList<string> filenames)
    {
        var sqlQuery = GenerateSqlQueryByFileNames(filenames);

        using (var connection = new SQLiteConnection(m_connectionString))
        {
            var result = connection.Query<MessageFile>(sqlQuery.Query, sqlQuery.Parameters).ToList();
            return result;
        }
    }

    /// <summary>
    /// Method for updating file information.
    /// </summary>
    public void UpdateMessageFileInfo()
    {
        // 
    }

    /// <summary>
    /// Method for generating an SQL query using a file name filter.
    /// </summary>
    private (string Query, DynamicParameters Parameters) GenerateSqlQueryByFileNames(IReadOnlyList<string> filenames)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(m_defaultSelectAllSQL);

        if (filenames != null && filenames.Count > 0)
        {
            var fileNamesFilter = string.Join(",", filenames.Select((f, index) => $"@FileName{index}"));
            stringBuilder.Append(" WHERE m.Name IN (").Append(fileNamesFilter).Append(")");

            var parameters = new DynamicParameters();
            for (int i = 0; i < filenames.Count; i++)
            {
                parameters.Add($"FileName{i}", filenames[i]);
            }

            stringBuilder.Append(";");
            return (stringBuilder.ToString(), parameters);
        }

        stringBuilder.Append(";");
        return (stringBuilder.ToString(), null);
    }
}
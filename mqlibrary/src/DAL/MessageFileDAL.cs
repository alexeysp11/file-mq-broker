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
    /// Method for updating file information.
    /// </summary>
    public void UpdateMessageFileInfo()
    {
        // 
    }

    /// <summary>
    /// Method for obtaining information about files.
    /// </summary>
    public IReadOnlyList<MessageFile> GetMessageFileInfo(IReadOnlyList<string> filenames, int pageSize, int pageNumber)
    {
        var sqlQuery = GenerateSqlQueryByFileNames(filenames, pageSize, pageNumber);

        using (var connection = new SQLiteConnection(m_connectionString))
        {
            var result = connection.Query<MessageFile>(sqlQuery.Query, sqlQuery.Parameters).ToList();
            return result;
        }
    }

    /// <summary>
    /// Method for generating an SQL query using a file name filter with pagination.
    /// </summary>
    private (string Query, DynamicParameters Parameters) GenerateSqlQueryByFileNames(IReadOnlyList<string> filenames, int pageSize, int pageNumber)
    {
        if (pageSize <= 0)
            throw new System.ArgumentException("Page size should be greater than zero", nameof(pageSize));
        if (pageNumber <= 0)
            throw new System.ArgumentException("Page number should be greater than zero", nameof(pageNumber));
        
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(m_defaultSelectAllSQL);

        var parameters = new DynamicParameters();
        if (filenames != null && filenames.Count > 0)
        {
            // Calculate the start and end indexes for the current page.
            int startIndex = (pageNumber - 1) * pageSize;
            int endIndex = Math.Min(startIndex + pageSize, filenames.Count);
            if (startIndex >= endIndex)
                throw new System.IndexOutOfRangeException("Failed pagination: start index could not be bigger than end index");

            // Generate the WHERE condition and dynamic parameters in a loop using the calculated indexes.
            // The loop is used to optimize the WHERE clause and reduce memory allocation while filtering.
            stringBuilder.Append(" WHERE m.Name IN (");
            for (int i = startIndex; i < endIndex; i++)
            {
                stringBuilder.Append($"@FileName{i}");
                if (i < endIndex - 1)
                    stringBuilder.Append(", ");
                
                parameters.Add($"FileName{i}", filenames[i]);
            }
            stringBuilder.Append(")");
        }
        
        stringBuilder.Append(";");
        return (stringBuilder.ToString(), parameters);
    }
}
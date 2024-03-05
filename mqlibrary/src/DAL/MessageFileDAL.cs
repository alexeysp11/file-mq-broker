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
    #region Private fields
    private readonly string m_connectionString;
    private readonly string m_defaultSelectAllSQL = "SELECT m.Name, m.MessageFileState FROM MessageFiles m";
    private readonly string m_defaultInsertMessageSQL = "INSERT INTO MessageFiles (Name, FileType, FileState) VALUES ";
    #endregion  // Private fields

    #region Constructors
    /// <summary>
    /// Default constructor.
    /// </summary>
    public MessageFileDAL(AppInitConfigs appInitConfigs)
    {
        m_connectionString = appInitConfigs.DbConnectionString;
    }
    #endregion  // Constructors

    #region Public methods
    /// <summary>
    /// Method for inserting the specified files in DB.
    /// </summary>
    public void InsertMessageFileState(IReadOnlyList<MessageFile> fileMessages)
    {
        var sqlQuery = GenerateInsertSqlByFileNames(fileMessages);

        using (var connection = new SQLiteConnection(m_connectionString))
        {
            connection.Execute(sqlQuery.Query, sqlQuery.Parameters);
        }
    }

    /// <summary>
    /// Method for updating state of the specified files.
    /// </summary>
    public void UpdateMessageFileState(IReadOnlyList<MessageFile> fileMessages)
    {
        var sqlQuery = GenerateUpdateSqlByFileNames(fileMessages);

        using (var connection = new SQLiteConnection(m_connectionString))
        {
            connection.Execute(sqlQuery.Query, sqlQuery.Parameters);
        }
    }

    /// <summary>
    /// Method for obtaining information about files.
    /// </summary>
    public IReadOnlyList<MessageFile> GetMessageFileInfo(IReadOnlyList<string> filenames, int pageSize, int pageNumber)
    {
        var sqlQuery = GenerateSelectSqlByFileNames(filenames, pageSize, pageNumber);

        using (var connection = new SQLiteConnection(m_connectionString))
        {
            var result = connection.Query<MessageFile>(sqlQuery.Query, sqlQuery.Parameters).ToList();
            return result;
        }
    }
    #endregion  // Public methods

    #region Private methods
    /// <summary>
    /// Method for generating an SQL query for inserting data about a file.
    /// </summary>
    private (string Query, DynamicParameters Parameters) GenerateInsertSqlByFileNames(IReadOnlyList<MessageFile> fileMessages)
    {
        if (fileMessages == null)
            throw new System.ArgumentNullException(nameof(fileMessages));
        
        var queryParameters = new DynamicParameters();
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(m_defaultInsertMessageSQL);

        for (int i = 0; i < fileMessages.Count; i++)
        {
            if (i > 0)
                stringBuilder.Append(", ");
            stringBuilder.Append($"(@file_{i}_Name, @file_{i}_FileType, @file_{i}_FileState)");

            queryParameters.Add($"file_{i}_Name", fileMessages[i].Name);
            queryParameters.Add($"file_{i}_FileType", fileMessages[i].MessageFileType);
            queryParameters.Add($"file_{i}_FileState", fileMessages[i].MessageFileState);
        }

        return (stringBuilder.ToString(), queryParameters);
    }

    /// <summary>
    /// Method for generating an SQL query for updating data about a file.
    /// </summary>
    private (string Query, DynamicParameters Parameters) GenerateUpdateSqlByFileNames(IReadOnlyList<MessageFile> fileMessages)
    {
        if (fileMessages == null)
            throw new System.ArgumentNullException(nameof(fileMessages));
        
        var queryParameters = new DynamicParameters();
        var stringBuilder = new StringBuilder();

        for (int i = 0; i < fileMessages.Count; i++)
        {
            stringBuilder.Append($"UPDATE MessageFiles SET MessageFileState = @MessageFileState_{i} WHERE Name = @Name_{i};");
            
            queryParameters.Add($"MessageFileState_{i}", fileMessages[i].MessageFileState);
            queryParameters.Add($"Name_{i}", fileMessages[i].Name);
        }

        return (stringBuilder.ToString(), queryParameters);
    }

    /// <summary>
    /// Method for generating an SQL query using a file name filter with pagination.
    /// </summary>
    private (string Query, DynamicParameters Parameters) GenerateSelectSqlByFileNames(IReadOnlyList<string> filenames, int pageSize, int pageNumber)
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
    #endregion  // Private methods
}
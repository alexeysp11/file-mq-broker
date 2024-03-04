using System.Data;
using System.Data.SQLite;
using System.Text;
using Dapper;
using FileMqBroker.MqLibrary.Models;

namespace FileMqBroker.MqLibrary.DAL;

/// <summary>
/// Allows you to add information about exceptions to the database.
/// </summary>
public class ExceptionDAL
{
    private readonly string m_connectionString;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public ExceptionDAL(AppInitConfigs appInitConfigs)
    {
        m_connectionString = appInitConfigs.DbConnectionString;
    }

    public void InsertExceptions(List<string> exceptions)
    {
        // 
    }
}
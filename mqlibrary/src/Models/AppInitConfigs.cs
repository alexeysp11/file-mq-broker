namespace FileMqBroker.MqLibrary.Models;

/// <summary>
/// Contains application configuration settings during startup.
/// </summary>
public class AppInitConfigs
{
    /// <summary>
    /// Connection string for DB.
    /// </summary>
    public string? DbConnectionString { get; set; }
    
    /// <summary>
    /// Directory name for requests.
    /// </summary>
    public string? RequestDirectoryName { get; set; }

    /// <summary>
    /// Directory name for responses.
    /// </summary>
    public string? ResponseDirectoryName { get; set; }

    /// <summary>
    /// Number of elements from the queue for one-time processing.
    /// </summary>
    public int OneTimeProcQueueElements { get; set; }
    
    /// <summary>
    /// Implementation of the duplicate request collapse.
    /// </summary>
    public DuplicateRequestCollapseType DuplicateRequestCollapseType { get; set; }
}
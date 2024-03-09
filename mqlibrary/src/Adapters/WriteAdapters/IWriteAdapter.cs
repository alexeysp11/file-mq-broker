namespace FileMqBroker.MqLibrary.Adapters.WriteAdapters;

/// <summary>
/// 
/// </summary>
public interface IWriteAdapter
{
    /// <summary>
    /// 
    /// </summary>
    void WriteMessage(string method, string path, string content);
}
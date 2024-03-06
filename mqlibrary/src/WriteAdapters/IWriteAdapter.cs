namespace FileMqBroker.MqLibrary.WriteAdapters;

/// <summary>
/// 
/// </summary>
public interface IWriteAdapter
{
    Task WriteMessageAsync(string message);
}
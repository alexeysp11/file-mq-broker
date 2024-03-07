namespace FileMqBroker.MqLibrary.Adapters.WriteAdapters;

/// <summary>
/// 
/// </summary>
public interface IWriteAdapter
{
    Task WriteMessageAsync(string message);
}
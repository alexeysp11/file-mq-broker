namespace FileMqBroker.MqLibrary.ReadAdapters;

/// <summary>
/// 
/// </summary>
public interface IReadAdapter
{
    Task<string> ReadMessageAsync();
}
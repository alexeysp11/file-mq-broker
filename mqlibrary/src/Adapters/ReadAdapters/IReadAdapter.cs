namespace FileMqBroker.MqLibrary.Adapters.ReadAdapters;

/// <summary>
/// 
/// </summary>
public interface IReadAdapter
{
    Task<string> ReadMessageAsync();
}
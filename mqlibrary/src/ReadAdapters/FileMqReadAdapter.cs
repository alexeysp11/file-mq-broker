namespace FileMqBroker.MqLibrary.ReadAdapters;

/// <summary>
/// 
/// </summary>
public class FileMqReadAdapter : IReadAdapter
{
    public async Task<string> ReadMessageAsync()
    {
        return string.Empty;
    }
}
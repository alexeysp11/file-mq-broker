namespace FileMqBroker.MqLibrary.ReadAdapters;

/// <summary>
/// 
/// </summary>
public class RabbitMqReadAdapter : IReadAdapter
{
    public async Task<string> ReadMessageAsync()
    {
        return string.Empty;
    }
}
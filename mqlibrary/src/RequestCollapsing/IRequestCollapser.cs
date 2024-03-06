namespace FileMqBroker.MqLibrary.QueueDispatchers;

/// <summary>
/// 
/// </summary>
public interface IRequestCollapser
{
    Task<string> CollapseRequestsAsync(List<string> requests);
}
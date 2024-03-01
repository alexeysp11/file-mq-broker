namespace FileMqBroker.MqLibrary.Models;

/// <summary>
/// Represents a message file.
/// </summary>
public struct MessageFile
{
    /// <summary>
    /// The name of the message file.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// The state of the message file.
    /// </summary>
    public MessageFileState MessageFileState { get; set; }
}
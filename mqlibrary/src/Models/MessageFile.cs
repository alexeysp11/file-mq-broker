namespace FileMqBroker.MqLibrary.Models;

/// <summary>
/// Represents a message file.
/// </summary>
public class MessageFile
{
    /// <summary>
    /// The name of the message file.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The content of the message file.
    /// </summary>
    public string Content { get; set; }
    
    /// <summary>
    /// The type of the message file.
    /// </summary>
    public MessageFileType MessageFileType { get; set; }
    
    /// <summary>
    /// The state of the message file.
    /// </summary>
    public MessageFileState MessageFileState { get; set; }
}
namespace FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;

/// <summary>
/// Interface for a filename generating.
/// </summary>
public interface IFileNameGeneration
{
    /// <summary>
    /// Generates a filename.
    /// </summary>
    string GetFileName(string method, string path);
}
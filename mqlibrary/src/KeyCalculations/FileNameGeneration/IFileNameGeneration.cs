namespace FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;

/// <summary>
/// Interface for key calculation.
/// </summary>
public interface IFileNameGeneration
{
    /// <summary>
    /// Calculates the hash based on the method and path.
    /// </summary>
    string CalculateHash(string method, string path);
}
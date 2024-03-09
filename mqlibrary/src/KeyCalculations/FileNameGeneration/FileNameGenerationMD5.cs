using System.IO;
using FileMqBroker.MqLibrary.KeyCalculations;

namespace FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;

/// <summary>
/// Generates a filename using MD5 hash calculation functionality.
/// </summary>
public class FileNameGenerationMD5 : IFileNameGeneration
{
    private IKeyCalculation m_keyCalculation;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public FileNameGenerationMD5(KeyCalculationMD5 keyCalculation)
    {
        m_keyCalculation = keyCalculation;
    }

    /// <summary>
    /// Generates a filename using MD5 hash.
    /// </summary>
    public string GetFileName(string method, string path)
    {
        var hash = CalculateHash(method, path);
        return $"{System.DateTime.Now}.{hash}";
    }

    /// <summary>
    /// Calculates the MD5 hash based on the method and path.
    /// </summary>
    private string CalculateHash(string method, string path)
    {
        var fullpath = Path.Combine(method, path);
        return m_keyCalculation.CalculateHash(fullpath);
    }
}
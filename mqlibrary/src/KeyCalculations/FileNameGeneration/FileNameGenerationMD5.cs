using System.Collections.Concurrent;
using System.IO;
using FileMqBroker.MqLibrary.KeyCalculations;

namespace FileMqBroker.MqLibrary.KeyCalculations.FileNameGeneration;

/// <summary>
/// Generates a filename using MD5 hash calculation functionality.
/// </summary>
public class FileNameGenerationMD5 : IFileNameGeneration
{
    private ConcurrentDictionary<string, string> m_fullpathHashDictionary;
    private IKeyCalculation m_keyCalculation;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public FileNameGenerationMD5(KeyCalculationMD5 keyCalculation)
    {
        m_fullpathHashDictionary = new ConcurrentDictionary<string, string>();
        m_keyCalculation = keyCalculation;
    }

    /// <summary>
    /// Generates a filename using MD5 hash.
    /// </summary>
    public string GetFileName(string method, string path)
    {
        var hash = CalculateHash(method, path);
        return $"{System.DateTime.Now.ToString("yyyyMMddHHmmssfff")}.{hash}.req";
    }

    /// <summary>
    /// Calculates the MD5 hash based on the method and path.
    /// </summary>
    public string CalculateHash(string method, string path)
    {
        var fullpath = Path.Combine(method, path);
        
        if (m_fullpathHashDictionary.ContainsKey(fullpath))
        {
            return m_fullpathHashDictionary[fullpath];
        }

        var hash = m_keyCalculation.CalculateHash(fullpath);
        m_fullpathHashDictionary.TryAdd(fullpath, hash);
        return hash;
    }
}
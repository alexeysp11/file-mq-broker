namespace FileMqBroker.MqLibrary.DirectoryOperations;

/// <summary>
/// 
/// </summary>
public class ReadWriteOperation
{
    /// <summary>
    /// 
    /// </summary>
    public void SaveRequestResponse(string method, string path, string response)
    {
        // string key = CalculateHash(method + path);
        string key = $"{method}-{path}";
        string requestFileName = key + ".req";
        string responseFileName = key + ".resp";

        File.WriteAllText(requestFileName, $"{method} {path}");
        File.WriteAllText(responseFileName, $"200\n{response}");
    }
}
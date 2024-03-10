using System.Text;

namespace FileMqBroker.MqLibrary.BackendService.FileContentGenerators;

/// <summary>
/// Provides functionality for generating a response to a received request as part of working with a message broker.
/// </summary>
public class MessageFileResponseGen
{
    /// <summary>
    /// Generates a response to a message by file name.
    /// </summary>
    public string GenerateResponseContent(string messageFileName)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append("Reponse code: ").Append(200).Append("\n").Append("Response for the file: ").Append(messageFileName);
        return stringBuilder.ToString();
    }
}
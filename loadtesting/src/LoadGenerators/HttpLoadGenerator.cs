using FileMqBroker.MqLibrary.LoadTesting.LoadCalculations;

namespace FileMqBroker.MqLibrary.LoadTesting.LoadGenerators;

/// <summary>
/// 
/// </summary>
public class HttpLoadGenerator : ILoadGenerator
{
    private ILoadCalculation m_loadCalculation;

    /// <summary>
    /// 
    /// </summary>
    public HttpLoadGenerator(ILoadCalculation loadCalculation)
    {
        m_loadCalculation = loadCalculation;
    }

    /// <summary>
    /// 
    /// </summary>
    public void GenerateLoad()
    {
        var currentLoad = m_loadCalculation.CalculateLoad();
        System.Console.WriteLine($"Current load: {currentLoad}");
    }
}
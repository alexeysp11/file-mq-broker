using FileMqBroker.MqLibrary.LoadTesting.LoadCalculations;

namespace FileMqBroker.MqLibrary.LoadTesting.LoadGenerators;

/// <summary>
/// 
/// </summary>
public class LibraryLoadGenerator : ILoadGenerator
{
    private ILoadCalculation m_loadCalculation;

    /// <summary>
    /// 
    /// </summary>
    public LibraryLoadGenerator(ILoadCalculation loadCalculation)
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

        // Нагрузка означает количество реквестов.
        // Соответственно, необходимо в цикле создать заданное количество клиентских классов.
        for (int i = 0; i < currentLoad; i++)
        {
            // 
        }
    }
}
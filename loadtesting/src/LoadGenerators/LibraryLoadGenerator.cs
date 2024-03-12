using FileMqBroker.MqLibrary.LoadTesting.LoadCalculations;

namespace FileMqBroker.MqLibrary.LoadTesting.LoadGenerators;

/// <summary>
/// Load generator that calls the message broker adapter as a library component.
/// </summary>
public class LibraryLoadGenerator : ILoadGenerator
{
    private ILoadCalculation m_loadCalculation;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public LibraryLoadGenerator(ILoadCalculation loadCalculation)
    {
        m_loadCalculation = loadCalculation;
    }

    /// <summary>
    /// Method that generates a load for the selected generator type.
    /// </summary>
    public void GenerateLoad()
    {
        var currentLoad = m_loadCalculation.CalculateLoad();
        System.Console.WriteLine($"Current load: {currentLoad}");

        // Load means the number of requests.
        // Accordingly, it is necessary to create a given number of client classes in a loop.
        for (int i = 0; i < currentLoad; i++)
        {
            // 
        }
    }
}
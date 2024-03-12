using FileMqBroker.MqLibrary.LoadTesting.Models;

namespace FileMqBroker.MqLibrary.LoadTesting.LoadCalculations;

/// <summary>
/// 
/// </summary>
public class OneTimeLoadCalculation : ILoadCalculation
{
    private int m_currentLoad;
    private LoadConfigParams m_loadConfigParams;

    /// <summary>
    /// Default constructor.
    /// </summary>
    public OneTimeLoadCalculation(LoadConfigParams loadConfigParams)
    {
        m_currentLoad = 0;
        m_loadConfigParams = loadConfigParams;
    }

    /// <summary>
    /// 
    /// </summary>
    public int CalculateLoad()
    {
        return 0;
    }
}
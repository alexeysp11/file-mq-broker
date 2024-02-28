namespace FileMqBroker.MqLibrary.KeyCalculations;

public interface IKeyCalculation
{
    string CalculateHash(string input);
}
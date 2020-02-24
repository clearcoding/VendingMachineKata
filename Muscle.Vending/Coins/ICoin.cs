namespace Muscle.Vending
{
    public interface ICoin
    {
        decimal Value { get; }
        decimal Weight { get; }
        decimal Size { get; }
    }
}
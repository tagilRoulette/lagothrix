namespace Logic;
public record struct PriceFork
{
    public PriceFork(double lowerLimit, double upperLimit)
    {
        LowerLimit = lowerLimit;
        UpperLimit = upperLimit;
    }

    public double LowerLimit { get; set; }
    public double UpperLimit { get; set; }
}

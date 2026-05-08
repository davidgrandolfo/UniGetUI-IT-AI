namespace UniGetUI.EndpointHygiene.Risk;

public sealed class RiskScoreResult
{
    public int Score { get; set; }
    public RiskLevel Level { get; set; } = RiskLevel.Low;
    public IReadOnlyList<string> Reasons { get; set; } = [];
    public RiskFactors Factors { get; set; } = new();
}

namespace UniGetUI.EndpointHygiene.Risk;

public sealed class RiskFactors
{
    public bool OutdatedVersion { get; set; }
    public bool UntrustedSource { get; set; }
    public bool CriticalApplication { get; set; }
    public bool HighRiskManager { get; set; }
    public bool RecentFailure { get; set; }
    public bool CveExposed { get; set; }
}

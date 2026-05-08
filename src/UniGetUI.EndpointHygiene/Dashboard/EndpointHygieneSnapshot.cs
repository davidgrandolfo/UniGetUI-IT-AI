namespace UniGetUI.EndpointHygiene.Dashboard;

public sealed class EndpointHygieneSnapshot
{
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
    public int OutdatedApplications { get; set; }
    public int HighOrCriticalRisks { get; set; }
    public int FailedUpdates { get; set; }
    public int NonCompliantSources { get; set; }
    public int GlobalHygieneScore { get; set; }
}

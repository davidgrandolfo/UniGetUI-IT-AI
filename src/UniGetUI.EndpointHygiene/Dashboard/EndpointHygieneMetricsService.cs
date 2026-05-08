using UniGetUI.EndpointHygiene.Models;
using UniGetUI.EndpointHygiene.Risk;
using UniGetUI.EndpointHygiene.Services;

namespace UniGetUI.EndpointHygiene.Dashboard;

public sealed class EndpointHygieneMetricsService
{
    private readonly IRiskScoreService _riskScoreService;

    public EndpointHygieneMetricsService(IRiskScoreService riskScoreService)
    {
        _riskScoreService = riskScoreService;
    }

    public EndpointHygieneSnapshot Build(InventorySnapshot snapshot, RiskContext context)
    {
        int highOrCritical = 0;
        int scoreAccumulator = 0;

        foreach (SoftwareInventoryItem item in snapshot.Items)
        {
            RiskScoreResult risk = _riskScoreService.Score(item, context);
            scoreAccumulator += risk.Score;
            if (risk.Level is RiskLevel.High or RiskLevel.Critical)
                highOrCritical++;
        }

        int maxScore = Math.Max(snapshot.Items.Count * HygieneDefaults.MaxRiskScorePerItem, 1);
        int normalized = Math.Max(0, 100 - (scoreAccumulator * 100 / maxScore));

        return new EndpointHygieneSnapshot
        {
            GeneratedAtUtc = DateTime.UtcNow,
            OutdatedApplications = snapshot.Items.Count(i => i.UpdateAvailable),
            HighOrCriticalRisks = highOrCritical,
            FailedUpdates = snapshot.Items.Count(i => i.HasRecentFailure),
            NonCompliantSources = snapshot.Items.Count(i =>
                context.ApprovedSources.Count > 0 && !context.ApprovedSources.Contains(i.SourceName)),
            GlobalHygieneScore = normalized,
        };
    }
}

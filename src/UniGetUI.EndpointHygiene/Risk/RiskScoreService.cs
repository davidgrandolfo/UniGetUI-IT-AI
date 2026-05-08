using UniGetUI.EndpointHygiene.Models;

namespace UniGetUI.EndpointHygiene.Risk;

public sealed class RiskScoreService : IRiskScoreService
{
    public RiskScoreResult Score(SoftwareInventoryItem item, RiskContext context)
    {
        ArgumentNullException.ThrowIfNull(item);
        ArgumentNullException.ThrowIfNull(context);

        int score = 0;
        List<string> reasons = [];
        RiskFactors factors = new();

        if (item.UpdateAvailable)
        {
            score += 20;
            factors.OutdatedVersion = true;
            reasons.Add("Installed version is outdated.");
        }

        if (context.ApprovedSources.Count > 0 && !context.ApprovedSources.Contains(item.SourceName))
        {
            score += 25;
            factors.UntrustedSource = true;
            reasons.Add("Package source is not approved by policy.");
        }

        if (context.CriticalApps.Contains(item.PackageId) || context.CriticalApps.Contains(item.Name))
        {
            score += 15;
            factors.CriticalApplication = true;
            reasons.Add("Application is marked as critical.");
        }

        if (context.HighRiskManagers.Contains(item.ManagerName))
        {
            score += 10;
            factors.HighRiskManager = true;
            reasons.Add("Package manager is marked as high risk.");
        }

        if (item.HasRecentFailure || context.FailedPackages.Contains(item.PackageId))
        {
            score += 20;
            factors.RecentFailure = true;
            reasons.Add("Recent install/update failure was detected.");
        }

        if (context.CveExposedPackages.Contains(item.PackageId))
        {
            score += 40;
            factors.CveExposed = true;
            reasons.Add("Known CVE exposure is associated with this package.");
        }

        if (context.BlockedPublishers.Contains(item.Publisher))
        {
            score += 35;
            factors.CveExposed = true;
            reasons.Add("Publisher is blocked by policy.");
        }

        return new RiskScoreResult
        {
            Score = score,
            Level = MapLevel(score),
            Reasons = reasons,
            Factors = factors,
        };
    }

    private static RiskLevel MapLevel(int score)
    {
        if (score >= 80)
            return RiskLevel.Critical;
        if (score >= 55)
            return RiskLevel.High;
        if (score >= 30)
            return RiskLevel.Medium;
        return RiskLevel.Low;
    }
}

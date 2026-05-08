namespace UniGetUI.EndpointHygiene.Services;

public static class HygieneDefaults
{
    public static readonly IReadOnlySet<string> HighRiskManagers =
        new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "scoop" };

    public const int MaxRiskScorePerItem = 100;
    public const string OperationHistoryPackageMarker = "Package=";
}

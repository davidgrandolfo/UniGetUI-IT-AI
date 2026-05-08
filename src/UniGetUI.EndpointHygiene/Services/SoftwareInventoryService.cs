using UniGetUI.Core.SettingsEngine;
using UniGetUI.EndpointHygiene.Interfaces;
using UniGetUI.EndpointHygiene.Models;
using UniGetUI.PackageEngine.Interfaces;
using UniGetUI.PackageEngine.PackageLoader;

namespace UniGetUI.EndpointHygiene.Services;

public sealed class SoftwareInventoryService : ISoftwareInventoryService
{
    public async Task<InventorySnapshot> BuildSnapshotAsync(CancellationToken cancellationToken = default)
    {
        var installed = InstalledPackagesLoader.Instance.Packages.ToArray();
        var updates = UpgradablePackagesLoader.Instance.Packages
            .GroupBy(p => p.GetHash())
            .ToDictionary(g => g.Key, g => g.First());

        var failedPackageIds = ParseFailedPackageIds();
        List<SoftwareInventoryItem> items = [];

        foreach (IPackage package in installed)
        {
            cancellationToken.ThrowIfCancellationRequested();

            updates.TryGetValue(package.GetHash(), out IPackage? updatePackage);
            if (!package.Details.IsPopulated)
                await package.Details.Load();

            items.Add(new SoftwareInventoryItem
            {
                Name = package.Name,
                PackageId = package.Id,
                InstalledVersion = package.VersionString,
                LatestVersion = updatePackage?.NewVersionString ?? package.VersionString,
                ManagerName = package.Manager.Name,
                SourceName = package.Source.Name,
                Publisher = package.Details.Publisher ?? string.Empty,
                InstallDate = package.Details.UpdateDate,
                UpdateAvailable = updatePackage is not null,
                InstallerHash = package.Details.InstallerHash,
                HasRecentFailure = failedPackageIds.Contains(package.Id),
            });
        }

        return new InventorySnapshot
        {
            GeneratedAtUtc = DateTime.UtcNow,
            Items = items,
        };
    }

    private static HashSet<string> ParseFailedPackageIds()
    {
        HashSet<string> failed = new(StringComparer.OrdinalIgnoreCase);
        foreach (string line in Settings.GetValue(Settings.K.OperationHistory).Split('\n'))
        {
            string clean = line.Trim();
            if (!clean.Contains(HygieneDefaults.OperationHistoryPackageMarker, StringComparison.OrdinalIgnoreCase))
                continue;

            bool failure = clean.Contains("failed", StringComparison.OrdinalIgnoreCase)
                || clean.Contains("error", StringComparison.OrdinalIgnoreCase);
            if (!failure)
                continue;

            int idx = clean.IndexOf(HygieneDefaults.OperationHistoryPackageMarker, StringComparison.OrdinalIgnoreCase);
            if (idx < 0)
                continue;
            string id = clean[(idx + HygieneDefaults.OperationHistoryPackageMarker.Length)..]
                .Split(' ', '\\', '\n', '\r')
                .FirstOrDefault() ?? "";
            if (!string.IsNullOrWhiteSpace(id))
                failed.Add(id.Trim());
        }

        return failed;
    }
}

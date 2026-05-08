using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UniGetUI.Core.Logging;
using UniGetUI.EndpointHygiene.Policy;
using UniGetUI.EndpointHygiene.Risk;
using UniGetUI.EndpointHygiene.Services;

namespace UniGetUI.Interface.SoftwarePages;

public sealed partial class EndpointHygieneDashboardPage : Page
{
    public EndpointHygieneDashboardPage()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        try
        {
            var snapshot = await EndpointHygieneBootstrap.Inventory.BuildSnapshotAsync();
            var policy = await EndpointHygieneBootstrap.PolicyStore.LoadAsync();

            var context = new RiskContext
            {
                ApprovedSources = policy.ApprovedSources,
                CriticalApps = policy.CriticalAppsRequiringApproval,
                BlockedPublishers = policy.BlockedPublishers,
                HighRiskManagers = new HashSet<string>(HygieneDefaults.HighRiskManagers, StringComparer.OrdinalIgnoreCase),
            };

            var dashboard = EndpointHygieneBootstrap.Metrics.Build(snapshot, context);

            GeneratedText.Text = $"Generated at {dashboard.GeneratedAtUtc:O}";
            OutdatedText.Text = $"Outdated applications: {dashboard.OutdatedApplications}";
            RiskText.Text = $"High/Critical risks: {dashboard.HighOrCriticalRisks}";
            FailuresText.Text = $"Failed updates: {dashboard.FailedUpdates}";
            SourcesText.Text = $"Non-compliant sources: {dashboard.NonCompliantSources}";
            ScoreText.Text = $"Global hygiene score: {dashboard.GlobalHygieneScore}";
        }
        catch (Exception ex)
        {
            Logger.Error("Failed to load endpoint hygiene dashboard page");
            Logger.Error(ex);
            GeneratedText.Text = "Dashboard unavailable.";
        }
    }
}

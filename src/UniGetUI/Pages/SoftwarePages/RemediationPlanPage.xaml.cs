using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UniGetUI.Core.Logging;
using UniGetUI.EndpointHygiene.Policy;
using UniGetUI.EndpointHygiene.Risk;
using UniGetUI.EndpointHygiene.Services;

namespace UniGetUI.Interface.SoftwarePages;

public sealed partial class RemediationPlanPage : Page
{
    public RemediationPlanPage()
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
            var riskContext = new RiskContext
            {
                ApprovedSources = policy.ApprovedSources,
                CriticalApps = policy.CriticalAppsRequiringApproval,
                BlockedPublishers = policy.BlockedPublishers,
                HighRiskManagers = new HashSet<string>(HygieneDefaults.HighRiskManagers, StringComparer.OrdinalIgnoreCase),
            };

            var plan = await EndpointHygieneBootstrap.Planner.BuildPlanAsync(snapshot, riskContext, policy);
            ActionsList.ItemsSource = plan.Actions;
            SubtitleText.Text = $"{plan.Actions.Count} action(s) generated.";
        }
        catch (Exception ex)
        {
            Logger.Error("Failed to load remediation plan page");
            Logger.Error(ex);
            SubtitleText.Text = "Failed to load remediation plan.";
            ActionsList.ItemsSource = null;
        }
    }
}

using UniGetUI.EndpointHygiene.Models;
using UniGetUI.EndpointHygiene.Risk;

namespace UniGetUI.EndpointHygiene.AI;

public sealed class AIRecommendationRequest
{
    public SoftwareInventoryItem Item { get; set; } = new();
    public RiskScoreResult RiskScore { get; set; } = new();
    public string[] RecentErrors { get; set; } = [];
}

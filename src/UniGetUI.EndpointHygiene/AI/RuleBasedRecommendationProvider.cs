using UniGetUI.EndpointHygiene.Risk;

namespace UniGetUI.EndpointHygiene.AI;

public sealed class RuleBasedRecommendationProvider : IAIRecommendationProvider
{
    public Task<AIRecommendationResponse> RecommendAsync(
        AIRecommendationRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ArgumentNullException.ThrowIfNull(request);

        string action = request.RiskScore.Level switch
        {
            RiskLevel.Low => "Update",
            RiskLevel.Medium => "UpdateWithApproval",
            RiskLevel.High => "ReviewAndUpdate",
            _ => "ManualReview",
        };

        string summary = request.RiskScore.Level switch
        {
            RiskLevel.Low => "Routine hygiene issue detected.",
            RiskLevel.Medium => "Moderate risk posture detected.",
            RiskLevel.High => "High risk exposure detected.",
            _ => "Critical risk exposure detected.",
        };

        AIRecommendationResponse response = new()
        {
            ProblemSummary = summary,
            Recommendation = $"Action suggested for {request.Item.Name}: {action}.",
            Justification = request.RiskScore.Reasons.Count == 0
                ? "No major risk factors were found."
                : string.Join(" ", request.RiskScore.Reasons),
            Confidence = request.RiskScore.Level is RiskLevel.Low ? 0.65 : 0.8,
            ProposedAction = action,
        };

        return Task.FromResult(response);
    }
}

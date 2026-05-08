namespace UniGetUI.EndpointHygiene.AI;

public sealed class AIRecommendationResponse
{
    public string ProblemSummary { get; set; } = "";
    public string Recommendation { get; set; } = "";
    public string Justification { get; set; } = "";
    public double Confidence { get; set; }
    public string ProposedAction { get; set; } = "Review";
}

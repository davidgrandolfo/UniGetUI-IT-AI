namespace UniGetUI.EndpointHygiene.AI;

public sealed class AIRecommendationOrchestrator
{
    private readonly IAIRecommendationProvider _provider;

    public AIRecommendationOrchestrator(IAIRecommendationProvider provider)
    {
        _provider = provider;
    }

    public Task<AIRecommendationResponse> GetRecommendationAsync(
        AIRecommendationRequest request,
        CancellationToken cancellationToken = default
    )
    {
        return _provider.RecommendAsync(request, cancellationToken);
    }
}

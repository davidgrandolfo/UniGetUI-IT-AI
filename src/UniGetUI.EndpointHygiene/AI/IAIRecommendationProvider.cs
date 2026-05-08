namespace UniGetUI.EndpointHygiene.AI;

public interface IAIRecommendationProvider
{
    Task<AIRecommendationResponse> RecommendAsync(
        AIRecommendationRequest request,
        CancellationToken cancellationToken = default
    );
}

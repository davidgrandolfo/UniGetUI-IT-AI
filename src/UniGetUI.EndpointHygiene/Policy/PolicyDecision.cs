namespace UniGetUI.EndpointHygiene.Policy;

public sealed class PolicyDecision
{
    public bool IsAllowed { get; set; }
    public bool RequiresApproval { get; set; }
    public IReadOnlyList<string> Reasons { get; set; } = [];
}

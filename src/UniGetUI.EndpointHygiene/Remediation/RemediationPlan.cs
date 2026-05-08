namespace UniGetUI.EndpointHygiene.Remediation;

public sealed class RemediationPlan
{
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
    public IReadOnlyList<RemediationAction> Actions { get; set; } = [];

    public IReadOnlyList<RemediationAction> Updates =>
        Actions.Where(a => a.ActionType == RemediationActionType.Update).ToArray();

    public IReadOnlyList<RemediationAction> Ignores =>
        Actions.Where(a => a.ActionType == RemediationActionType.Ignore).ToArray();

    public IReadOnlyList<RemediationAction> Uninstalls =>
        Actions.Where(a => a.ActionType == RemediationActionType.Uninstall).ToArray();
}

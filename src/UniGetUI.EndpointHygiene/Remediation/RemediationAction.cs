using UniGetUI.EndpointHygiene.Risk;

namespace UniGetUI.EndpointHygiene.Remediation;

public sealed class RemediationAction
{
    public string PackageId { get; set; } = "";
    public string PackageName { get; set; } = "";
    public RemediationActionType ActionType { get; set; }
    public RiskLevel RiskLevel { get; set; }
    public bool RequiresAdminApproval { get; set; }
    public bool RollbackSupported { get; set; }
    public string Reason { get; set; } = "";
    public string AiRecommendation { get; set; } = "";
}

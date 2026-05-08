using UniGetUI.EndpointHygiene.Models;
using UniGetUI.EndpointHygiene.Remediation;

namespace UniGetUI.EndpointHygiene.Policy;

public interface IPolicyEngine
{
    PolicyDecision Evaluate(SoftwareInventoryItem item, RemediationAction action, PolicyDefinition policy);
}

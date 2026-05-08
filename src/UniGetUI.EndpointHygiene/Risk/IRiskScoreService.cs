using UniGetUI.EndpointHygiene.Models;

namespace UniGetUI.EndpointHygiene.Risk;

public interface IRiskScoreService
{
    RiskScoreResult Score(SoftwareInventoryItem item, RiskContext context);
}

using UniGetUI.EndpointHygiene.Models;

namespace UniGetUI.EndpointHygiene.Interfaces;

public interface ISoftwareInventoryService
{
    Task<InventorySnapshot> BuildSnapshotAsync(CancellationToken cancellationToken = default);
}

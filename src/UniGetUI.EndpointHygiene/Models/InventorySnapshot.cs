namespace UniGetUI.EndpointHygiene.Models;

public sealed class InventorySnapshot
{
    public DateTime GeneratedAtUtc { get; set; } = DateTime.UtcNow;
    public IReadOnlyList<SoftwareInventoryItem> Items { get; set; } = [];
}

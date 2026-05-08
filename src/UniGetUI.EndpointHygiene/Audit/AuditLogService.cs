using System.Text.Json;
using UniGetUI.Core.Data;

namespace UniGetUI.EndpointHygiene.Audit;

public sealed class AuditLogService
{
    private readonly string _auditPath = Path.Combine(
        CoreData.UniGetUIDataDirectory,
        "EndpointHygiene",
        "audit-log.jsonl"
    );

    public async Task AppendAsync(AuditLogEntry entry, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_auditPath)!);
        string json = JsonSerializer.Serialize(entry);
        await File.AppendAllTextAsync(_auditPath, json + Environment.NewLine, cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLogEntry>> ReadAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_auditPath))
            return [];

        List<AuditLogEntry> entries = [];
        foreach (string line in await File.ReadAllLinesAsync(_auditPath, cancellationToken))
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;
            AuditLogEntry? entry = JsonSerializer.Deserialize<AuditLogEntry>(line);
            if (entry is not null)
                entries.Add(entry);
        }

        return entries;
    }

    public string GetAuditPath() => _auditPath;
}

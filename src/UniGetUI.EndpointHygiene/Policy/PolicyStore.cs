using System.Text.Json;
using UniGetUI.Core.Data;
using UniGetUI.Core.Logging;

namespace UniGetUI.EndpointHygiene.Policy;

public sealed class PolicyStore
{
    private readonly string _policyPath = Path.Combine(
        CoreData.UniGetUIDataDirectory,
        "EndpointHygiene",
        "policy.json"
    );

    public async Task<PolicyDefinition> LoadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(_policyPath))
                return new PolicyDefinition();

            await using FileStream stream = File.OpenRead(_policyPath);
            PolicyDefinition? policy = await JsonSerializer.DeserializeAsync<PolicyDefinition>(
                stream,
                cancellationToken: cancellationToken
            );
            return policy ?? new PolicyDefinition();
        }
        catch (Exception ex)
        {
            Logger.Warn("Could not load endpoint hygiene policy, using defaults.");
            Logger.Warn(ex);
            return new PolicyDefinition();
        }
    }

    public async Task SaveAsync(PolicyDefinition policy, CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_policyPath)!);
        await using FileStream stream = File.Create(_policyPath);
        await JsonSerializer.SerializeAsync(stream, policy, cancellationToken: cancellationToken);
    }
}

namespace SharedPipelines.DockerSample;

/// <summary>
/// Minimal HTTP probe used by the Docker integration tests. The base address
/// comes from an environment variable that the pipeline exports before the
/// tests run.
/// </summary>
public sealed class ServiceProbe : IDisposable
{
    /// <summary>Environment variable holding the service base address.</summary>
    public const string BaseAddressVariable = "SERVICE_URL";

    private readonly HttpClient _client;

    /// <summary>
    /// Creates a probe for <paramref name="baseAddress"/> or, when it is not
    /// supplied, for the address in <see cref="BaseAddressVariable"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// No base address was supplied and the environment variable is not set.
    /// </exception>
    public ServiceProbe(string? baseAddress = null)
    {
        var address = baseAddress ?? Environment.GetEnvironmentVariable(BaseAddressVariable);
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new InvalidOperationException(
                $"Set {BaseAddressVariable} or pass a base address to {nameof(ServiceProbe)}.");
        }

        _client = new HttpClient
        {
            BaseAddress = new Uri(address, UriKind.Absolute),
            Timeout = TimeSpan.FromSeconds(10),
        };
    }

    /// <summary>Gets the configured relative path from the service.</summary>
    public Task<HttpResponseMessage> GetAsync(
        string path = "",
        CancellationToken cancellationToken = default) =>
        _client.GetAsync(path, cancellationToken);

    /// <inheritdoc />
    public void Dispose() => _client.Dispose();
}

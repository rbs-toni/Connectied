using Connectied.Application.Persistence;
using System;
using System.Linq;
using System.Text.Json;

namespace Connectied.Server.Infrastructure;
sealed class GuestHttpClient : IGuestHttpClient
{
    readonly HttpClient _client;
    readonly JsonSerializerOptions _jsonOptions;
    public GuestHttpClient(HttpClient client)
    {
        _client = client;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        _jsonOptions.Converters.Add(new DummyGuestJsonConverter());
    }

    public async Task<IReadOnlyCollection<DummyGuest>> GetLatestGuests(CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync("0e23-17c0-49ad-a57e", cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IReadOnlyCollection<DummyGuest>>(_jsonOptions, cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize guest lists.");
    }
}

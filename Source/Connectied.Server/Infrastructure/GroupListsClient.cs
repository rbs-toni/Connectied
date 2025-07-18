﻿using Connectied.Application.GuestList;
using Connectied.Application.GuestList.Converters;
using System;
using System.Linq;
using System.Text.Json;

namespace Connectied.Server.Infrastructure;
public class GroupListsClient : IGroupListsClient
{
    readonly HttpClient _client;
    readonly JsonSerializerOptions _jsonOptions;
    public GroupListsClient(HttpClient client)
    {
        _client = client;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
        };
        _jsonOptions.Converters.Add(new GuestDtoJsonConverter());
    }

    public async Task<IReadOnlyCollection<GuestDto>> GetLatestGroupLists(CancellationToken cancellationToken = default)
    {
        var response = await _client.GetAsync("0e23-17c0-49ad-a57e", cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IReadOnlyCollection<GuestDto>>(_jsonOptions, cancellationToken)
               ?? throw new InvalidOperationException("Failed to deserialize guest lists.");
    }
}

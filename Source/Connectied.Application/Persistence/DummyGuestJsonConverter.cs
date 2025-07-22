using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Connectied.Application.Persistence;
public class DummyGuestJsonConverter : JsonConverter<DummyGuest>
{
    public override DummyGuest Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader).RootElement;

        return new DummyGuest
        {
            Id = json.GetProperty("id").GetString() ?? string.Empty,
            Name = json.GetProperty("name").GetString() ?? throw new ArgumentNullException(),
            Group = json.GetProperty("group").GetString() ?? string.Empty,
            Event1Quota = GetIntOrDefault(json, "event1quota"),
            Event2Quota = GetIntOrDefault(json, "event2quota"),
            Event1RSVP = GetIntOrDefault(json, "event1rsvp"),
            Event2RSVP = GetIntOrDefault(json, "event2rsvp"),
            Event1Attendance = GetIntOrDefault(json, "event1attend"),
            Event2Attendance = GetIntOrDefault(json, "event2attend"),
            Event1Angpao = GetIntOrDefault(json, "event1angpaoCount"),
            Event2Angpao = GetIntOrDefault(json, "event2angpaoCount"),
            Event1Gift = GetIntOrDefault(json, "event1giftCount"),
            Event2Gift = GetIntOrDefault(json, "event2giftCount"),
            Event1Souvenir = GetIntOrDefault(json, "event1souvenir"),
            Event2Souvenir = GetIntOrDefault(json, "event2souvenir"),
            Notes = json.TryGetProperty("notes", out var n) ? n.GetString() : null
        };
    }
    public override void Write(Utf8JsonWriter writer, DummyGuest value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
    static int GetIntOrDefault(JsonElement json, string prop)
    {
        return json.TryGetProperty(prop, out var val) && val.ValueKind != JsonValueKind.Null
            ? val.GetInt32()
            : 0;
    }
}

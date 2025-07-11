using System.Text.Json.Serialization;
using System.Text.Json;

namespace Connectied.Application.GuestList.Converters;
public class GuestDtoJsonConverter : JsonConverter<GuestDto>
{
    public override GuestDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader).RootElement;

        return new GuestDto
        {
            Id = json.GetProperty("id").GetString() ?? "",
            Name = json.GetProperty("name").GetString() ?? "",
            Group = json.TryGetProperty("group", out var g) ? g.GetString() : null,
            Event1Quota = GetIntOrDefault(json, "event1quota"),
            Event2Quota = GetIntOrDefault(json, "event2quota"),
            Event1Rsvp = GetIntOrDefault(json, "event1rsvp"),
            Event2Rsvp = GetIntOrDefault(json, "event2rsvp"),
            Event1Attend = GetIntOrDefault(json, "event1attend"),
            Event2Attend = GetIntOrDefault(json, "event2attend"),
            Event2AngpaoCount = GetIntOrDefault(json, "event2angpaoCount"),
            Event2GiftCount = GetIntOrDefault(json, "event2giftCount"),
            Event2Souvenir = GetIntOrDefault(json, "event2souvenir"),
            Notes = json.TryGetProperty("notes", out var n) ? n.GetString() : null
        };
    }
    public override void Write(Utf8JsonWriter writer, GuestDto value, JsonSerializerOptions options)
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

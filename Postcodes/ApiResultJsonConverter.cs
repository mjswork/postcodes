using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Postcodes.Models.Response;
using Postcodes.Models;
using Postcodes.Models.ThirdParty;

public class ApiResultJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        try
        {
            var jObject = JObject.Load(reader);
            var json = JsonConvert.DeserializeObject<PostcodeSingleResult>(jObject.ToString());

            return new PostcodeResponse
            {
                Postcode = json?.result?.postcode ?? null,
                Coordinates = new Coordinate
                {
                    Latitude = json?.result?.latitude.ToString() ?? null,
                    Longitude = json?.result?.longitude.ToString() ?? null
                }
            };
        }
        catch (Exception)
        {
            throw new Exception("Could not process Api results");
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}
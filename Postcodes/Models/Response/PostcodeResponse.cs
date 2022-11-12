using Newtonsoft.Json;
using Postcodes.Services;

namespace Postcodes.Models.Response
{
    [JsonConverter(typeof(ApiResultJsonConverter))]
    public class PostcodeResponse
    {
        public string? Postcode { get; set; }
        public Coordinate? Coordinates { get; set; } = null;
    }
}

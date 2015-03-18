using Newtonsoft.Json;

namespace Chicken.Services.Security
{
    [JsonObject]
    public class VkAccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}

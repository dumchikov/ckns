using Newtonsoft.Json;

namespace Chicken.Domain.Models
{
    [JsonObject]
    public class Attachment : Entity
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "photo")]
        public virtual Photo Photo { get; set; }

        public virtual Post Post { get; set; }
    }
}

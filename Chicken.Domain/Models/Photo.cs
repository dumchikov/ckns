using Newtonsoft.Json;

namespace Chicken.Domain.Models
{
    [JsonObject]
    public class Photo : Entity
    {
        [JsonProperty(PropertyName = "id")]
        public string PhotoId { get; set; }

        [JsonProperty(PropertyName = "photo_75")]
        public string Photo75Url { get; set; }

        [JsonProperty(PropertyName = "photo_130")]
        public string Photo130Url { get; set; }

        [JsonProperty(PropertyName = "photo_604")]
        public string Photo604Url { get; set; }

        [JsonProperty(PropertyName = "photo_807")]
        public string Photo807Url { get; set; }

        [JsonProperty(PropertyName = "photo_1280")]
        public string Photo1280Url { get; set; }

        //public virtual Attachment Attachment { get; set; }
    }
}
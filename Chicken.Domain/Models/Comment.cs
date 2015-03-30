using System;
using Chicken.Domain.Tools;
using Newtonsoft.Json;

namespace Chicken.Domain.Models
{
    [JsonObject]
    public class Comment : Entity
    {
        [JsonProperty(PropertyName = "id")]
        public string CommentId { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(UnixTimeStampToDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "from_id")]
        public int ProfileId { get; set; }

        public virtual Post Post { get; set; }

        public virtual User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Chicken.Domain.Tools;
using Newtonsoft.Json;

namespace Chicken.Domain.Models
{
    [JsonObject]
    public class Post : Entity
    {
        [JsonProperty(PropertyName = "id")]
        public long PostId { get; set; }

        [JsonProperty(PropertyName = "date")]
        [JsonConverter(typeof(UnixTimeStampToDateTimeConverter))]
        public DateTime Date { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "comments")]
        public dynamic CommentsObject { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "likes")]
        public dynamic LikesObject { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }  

        public int CommentsCount { get; set; }

        public int LikesCount { get; set; }

        [JsonProperty(PropertyName = "attachments")]
        public virtual ICollection<Attachment> Attachments { get; set; }

        public void UpdateLikesAndComments()
        {
            CommentsCount = CommentsObject.count;
            LikesCount = LikesObject.count;
        }
    }
}

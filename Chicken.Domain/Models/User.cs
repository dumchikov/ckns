using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Chicken.Domain.Models
{
    [JsonObject]
    public class User : Entity
    {
        [JsonProperty(PropertyName = "id")]
        public int ProfileId { get; set; }

        [JsonProperty(PropertyName = "photo_100")]
        public string Avatar { get; set; }

        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        [JsonProperty(PropertyName = "screen_name")]
        public string ScreenName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        [NotMapped]
        public string Link
        {
            get
            {
                return string.Format("http://vk.com/{0}", ScreenName);
            }
        }
    }
}

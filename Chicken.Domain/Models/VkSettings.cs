namespace Chicken.Domain.Models
{
    public class VkSettings : Entity
    {
        public string AppId { get; set; }

        public string Secret { get; set; }

        public string AccessToken { get; set; }
    }
}

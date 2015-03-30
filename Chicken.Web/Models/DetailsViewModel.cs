using System.Collections.Generic;
using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class DetailsViewModel
    {
        public string Text { get; set; }

        public IList<string> Photos { get; set; }

        public static DetailsViewModel Map(Post post)
        {
            var model = new DetailsViewModel
                {
                    Photos = new List<string>(),
                    Text = post.Text
                };

            if (post.Attachments != null && post.Attachments.Any())
            {
                foreach (var attachment in post.Attachments)
                {
                    if (attachment.Photo != null)
                    {
                        model.Photos.Add(attachment.Photo.Photo604Url);
                    }
                }
            }

            return model;
        }
    }
}
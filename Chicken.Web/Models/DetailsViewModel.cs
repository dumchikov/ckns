using System.Collections.Generic;
using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class DetailsViewModel
    {
        public string Text { get; set; }

        public IList<string> Photos { get; set; }

        public IList<CommentViewModel> Comments { get; set; }

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

            model.Comments = post.Comments.Select(x => new CommentViewModel
                {
                    Avatar = "http://cs622227.vk.me/v622227293/26f62/6Wytefh9Vmc.jpg",
                    Link = "http://vk.com/b_r_u_t_a_l_i_t_y",
                    Name = "Остап Белых",
                    Text = x.Text
                }).ToList();

            return model;
        }
    }
}
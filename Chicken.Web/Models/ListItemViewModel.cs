using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class ListItemViewModel
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public int Comments { get; set; }

        public int Likes { get; set; }

        public static ListItemViewModel Map(Post post)
        {
            var model = new ListItemViewModel
                {
                    Id = post.Id,
                    Comments = post.CommentsCount,
                    Likes = post.LikesCount,
                    Photo = post.Avatar
                };

            return model;
        }
    }
}
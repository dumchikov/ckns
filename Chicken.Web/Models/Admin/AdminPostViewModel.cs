using System.Collections.Generic;
using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Web.Models.Admin
{
    public class AdminPostViewModel
    {
        public int Id { get; set; }

        public string Date { get; set; }

        public bool IsSpam { get; set; }

        public string Text { get; set; }

        public string Avatar { get; set; }

        public static AdminPostViewModel Map(Post post)
        {
            var model = new AdminPostViewModel
                {
                    Id = post.Id,
                    Text = post.Text,
                    Date = string.Format("{0:dd/MM/yyyy HH:mm}", post.Date),
                    Avatar = post.Avatar,
                    IsSpam = post.IsSpam
                };

            return model;
        }
    }
}
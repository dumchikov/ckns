using System;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class CommentViewModel
    {
        public string Avatar { get; set; }

        public string Link { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public string Date { get; set; }

        public static CommentViewModel Map(Comment comment)
        {
            return new CommentViewModel
                {
                    Date = String.Format("{0:dd/MM/yyyy HH:mm}", comment.Date),
                    Text = comment.Text,
                    Avatar = comment.User.Avatar,
                    Link = comment.User.Link,
                    Name = comment.User.FullName
                };
        }
    }
}
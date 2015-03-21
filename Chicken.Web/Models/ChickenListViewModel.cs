﻿using System.Linq;
using Chicken.Domain.Models;

namespace Chicken.Web.Models
{
    public class ChickenListViewModel
    {
        public int Id { get; set; }

        public string Photo { get; set; }

        public int Comments { get; set; }

        public int Likes { get; set; }

        public static ChickenListViewModel Map(Post post)
        {
            var model = new ChickenListViewModel
                {
                    Id = post.Id,
                    Comments = post.CommentsCount,
                    Likes = post.LikesCount,
                    Photo = string.Empty
                };

            if (post.Attachments != null && post.Attachments.Any())
            {
                var att = post.Attachments.First();
                if (att.Photo != null)
                {
                    model.Photo = att.Photo.Photo130Url;
                }
            }

            return model;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using System.Linq;

namespace Chicken.Services
{
    public class ChickenService : IChickenService
    {
        private readonly NotificationService _notificationService;

        private readonly VkApi _api = new VkApi();

        private readonly IRepository<Post> _posts;

        public ChickenService(IRepository<Post> posts, NotificationService notificationService)
        {
            _posts = posts;
            _notificationService = notificationService;
        }

        public Post GetPost(int id)
        {
            var chicken = _posts.GetById(id);
            return chicken;
        }

        public int GetPostsCount()
        {
            return _posts.Query().Count();
        }

        public int GetSpamCount()
        {
            return _posts.Query().Count(x => x.IsSpam);
        }

        public IEnumerable<Post> GetPosts(int skip, int take, bool withSpam = false)
        {
            var query = _posts.Query();
            if (!withSpam) { query = query.Where(x => !x.IsSpam); }
            query = query.OrderByDescending(x => x.Date).Skip(skip).Take(take);
            return query;
        }

        public IEnumerable<Comment> GetComments(int id)
        {
            var post = _posts.GetById(id);
            var comments = post.Comments;
            if (comments != null && comments.Any())
            {
                return comments;
            }

            comments = _api.GetComments(100, 0, post.PostId).Where(x => !string.IsNullOrEmpty(x.Text)).ToList();
            var userIds = comments.Select(x => x.ProfileId).ToList();
            var users = _api.GetUsers(userIds).ToList();
            foreach (var comment in comments)
            {
                comment.User = users.Single(x => x.ProfileId == comment.ProfileId);
            }

            post.Comments = comments;
            _posts.Edit(post);
            _posts.Save();
            return comments;
        }

        public void AddNewPosts()
        {
            var posts = GetNewPosts()
                .Where(x =>
                !string.IsNullOrEmpty(x.Text)
                && x.Attachments != null
                && x.Attachments.Any())
                .ToList();

            var regex = new Regex(@"\*\*Информаци(.|\n)*СУДАРИ\n*");
            foreach (var post in posts)
            {
                try
                {
                    SetAvatar(post);
                    ModifyText(post, regex);
                    this._posts.Add(post);
                }
                catch
                {
                }
            }

            this._posts.Save();
            _notificationService.Notify(posts.Count());
        }

        private IEnumerable<Post> GetNewPosts()
        {
            var posts = new List<Post>();
            var savedChickens = this._posts.Query().Select(x => x.PostId).ToList();
            for (var i = 0; i < 1000; i++)
            {
                var responsePosts = _api.GetPosts(i * 100, 100);
                var newPosts = responsePosts.Where(x => !savedChickens.Contains(x.PostId)).ToList();

                if (!newPosts.Any())
                {
                    break;
                }

                posts.AddRange(newPosts);
            }

            return posts;
        }

        private static void ModifyText(Post post, Regex regex)
        {
            post.Text = regex.Replace(post.Text, string.Empty);
        }

        private static void SetAvatar(Post post)
        {
            if (post.Attachments != null)
            {
                string avatar;

                var photos =
                    post
                    .Attachments
                    .Where(x => x.Photo != null && x.Photo.Photo604Url != null)
                    .Select(x => x.Photo.Photo604Url)
                    .ToList();

                if (!photos.Any())
                {
                    return;
                }

                if (photos.Count() == 1)
                {
                    avatar = photos.First();
                    post.IsSpam = true;
                }
                else
                {
                    avatar =
                        photos.
                        Select(x => new
                        {
                            Photo = x,
                            Delta = GetPhotoDelta(x)
                        })
                        .OrderBy(x => x.Delta)
                        .First().Photo;
                }

                post.Avatar = avatar;
            }
        }

        private static int GetPhotoDelta(string photo)
        {
            var img = GetImageFromUrl(photo);
            var delta = Math.Abs(img.Height - img.Width);
            return delta;
        }

        private static Image GetImageFromUrl(string url)
        {
            using (var webClient = new WebClient())
            {
                return ByteArrayToImage(webClient.DownloadData(url));
            }
        }

        private static Image ByteArrayToImage(byte[] fileBytes)
        {
            using (var stream = new MemoryStream(fileBytes))
            {
                return Image.FromStream(stream);
            }
        }
    }
}

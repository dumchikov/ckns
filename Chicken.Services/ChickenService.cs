using System.Collections.Generic;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using System.Linq;

namespace Chicken.Services
{
    public class ChickenService
    {
        private readonly VkApi _api = new VkApi();

        private readonly IRepository<Post> _posts;

        public ChickenService(IRepository<Post> posts)
        {
            _posts = posts;
        }

        public Post GetPost(int id)
        {
            var chicken = _posts.GetById(id);
            return chicken;
        }

        public IEnumerable<Post> GetPosts(int skip, int take)
        {
            var chickens =
                _posts
                .Query()
                .OrderByDescending(x => x.Date)
                .Skip(skip)
                .Take(take);
            return chickens;
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
            return comments.OrderByDescending(x=>x.Date);
        }

        public void AddNewPosts()
        {
            var posts = GetNewPosts()
                .Where(
                x =>
                    !string.IsNullOrEmpty(x.Text)
                    && x.Attachments != null
                    && x.Attachments.Any());

            foreach (var post in posts)
            {
                this._posts.Add(post);
                this._posts.Save();
            }
        }

        private IEnumerable<Post> GetNewPosts()
        {
            var posts = new List<Post>();
            var savedChickens = this._posts.Query().Select(x => x.PostId).ToList();
            for (var i = 0; i < 2; i++)
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
    }
}

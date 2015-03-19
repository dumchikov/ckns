using System.Collections.Generic;
using System.Net;
using System.Text;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Chicken.Services
{
    public class ChickenService
    {
        private const string WallGetUrl =
            "https://api.vk.com/method/wall.get?v=5.7&domain=koko_kharkov&count={0}&offset={1}&filter=all&access_token={2}";

        private readonly IRepository<Post> _posts;

        public ChickenService(IRepository<Post> posts)
        {
            _posts = posts;
        }

        public IEnumerable<Post> GetExisingChickens(int skip, int take)
        {
            var chickens = _posts.Query().OrderBy(x => x.Date).Skip(skip).Take(take);
            return chickens;
        }

        public Post GetChicken(int id)
        {
            var chicken = _posts.GetById(id);
            return chicken;
        }

        public void AddNewPosts(string token)
        {
            var posts = GetNewPosts(token);

            foreach (var post in posts)
            {
                this._posts.Add(post); 
                this._posts.Save();
            }
        }

        #region Helpers
        private static IEnumerable<Post> GetChickens(string token, int skip, int take)
        {
            var url = string.Format(WallGetUrl, take, skip, token);
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var response = webClient.DownloadString(url);
            var items = JObject
                .Parse(response)
                .SelectToken("response")
                .SelectToken("items");

            var posts = items.ToObject<IEnumerable<Post>>().ToList();
            posts.ToList().ForEach(x => x.UpdateLikesAndComments());
            return posts;
        }

        private IEnumerable<Post> GetNewPosts(string token)
        {
            var posts = new List<Post>();
            var savedChickens = this._posts.Query().Select(x => x.PostId).ToList();

            for (var i = 0; i < 1000; i++)
            {
                var responsePosts = GetChickens(token,  i * 100, 100);
                var newPosts = responsePosts.Where(x => !savedChickens.Contains(x.PostId)).ToList();

                if (!newPosts.Any())
                {
                    break;
                }

                posts.AddRange(newPosts);
            }

            return posts;
        }
        #endregion
    }
}

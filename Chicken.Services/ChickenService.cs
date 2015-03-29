using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Chicken.Services
{
    public class ChickenService
    {
        private const string GetPostsUrl =
            "https://api.vk.com/method/wall.get?v=5.7&domain=koko_kharkov&count={0}&offset={1}&filter=all&access_token={2}";

        private readonly IRepository<Post> _posts;

        public ChickenService(IRepository<Post> posts)
        {
            _posts = posts;
        }

        public IEnumerable<Post> GetExisingChickens(int skip, int take)
        {
            var chickens = _posts.Query().OrderByDescending(x => x.Date).Skip(skip).Take(take);
            return chickens;
        }

        public Post GetChicken(int id)
        {
            var chicken = _posts.GetById(id);
            return chicken;
        }

        public void AddNewPosts(string token)
        {
            var posts =
                GetNewPosts(token)
                    .ToList()
                    .Where(x => !string.IsNullOrEmpty(x.Text) && x.Attachments != null && x.Attachments.Any());

            foreach (var post in posts)
            {
                this._posts.Add(post); 
            }

            this._posts.Save();
        }

        public void RemoveNonChickenPosts()
        {
            const string adminText = "Информацию присылают участники, администрация группы ответственности не несет! Мнение автора и администрации может не совпадать! Истории вымышленные, любое сходство чисто случайно, в случае совпадения писать администратору сообщества ";

            var nonChickenPosts = this._posts.Query().Where(x => !x.Text.Contains(adminText)).ToList();
            foreach (var nonChickenPost in nonChickenPosts)
            {
                _posts.Delete(nonChickenPost);
            }

            _posts.Save();
        }

        public ICollection<Comment> GetComments(int count, int offset, long postId, int ownerId, string accessToken)
        {
            accessToken = "4678659ecbffd3567799b72c33b3fc540da9dc49c02bf65ff0b3cf29a1894055d2b1013aafa39878b130b";
            const string secret = "XRxEAyihyEZOlyFcHTpU";
            const string domain = "https://api.vk.com";
            var requestString = string.Format("/method/wall.getComments?" +
                                    "v=5.29&" +
                                    "extended=1&" +
                                    "count={0}&" +
                                    "offset={1}&" +
                                    "post_id={2}&" +
                                    "owner_id=-{3}&" +
                                    "access_token={4}",
                                    count, offset, postId, ownerId, accessToken);
            var sig = GetMD5Hash(requestString + secret);
            var url = string.Format("{0}{1}&sig={2}", domain, requestString, sig);
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var response = webClient.UploadString(url, string.Empty);
            var items = JObject.Parse(response).SelectToken("response").SelectToken("items");
            var comments = items.ToObject<IEnumerable<Comment>>().ToList();
            return comments;
        }

        public void UpdateComments()
        {
            var posts = _posts.Query().ToList();
            foreach (var post in posts)
            {
                var comments = GetComments(100, 0, post.PostId, 65470032, "");
                post.Comments = comments;
                _posts.Edit(post);
                if (posts.IndexOf(post) % 10 == 0)
                {
                    _posts.Save();
                }
            }
        }

        #region Helpers
        private IEnumerable<Post> GetChickens(string token, int skip, int take)
        {
            var accessToken = "4678659ecbffd3567799b72c33b3fc540da9dc49c02bf65ff0b3cf29a1894055d2b1013aafa39878b130b";
            const string secret = "XRxEAyihyEZOlyFcHTpU";
            const string domain = "https://api.vk.com";
            var requestString = string.Format("/method/wall.get?" +
                                    "v=5.29&" +
                                    "count={0}&" +
                                    "offset={1}&" +
                                    "filter=all&" +
                                    "domain=koko_kharkov&" +
                                    "access_token={2}",
                                    take, skip, accessToken);
            var sig = GetMD5Hash(requestString + secret);

            var url = string.Format("{0}{1}&sig={2}", domain, requestString, sig);
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
            for (var i = 0; i < 5; i++)
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

        public string GetMD5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        #endregion
    }
}

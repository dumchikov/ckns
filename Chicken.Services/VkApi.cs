using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Chicken.Domain.Models;
using Newtonsoft.Json.Linq;

namespace Chicken.Services
{
    public class VkApi
    {
        private int ownerId = 65470032;
        private string domain = "https://api.vk.com";
        private string secret = "XRxEAyihyEZOlyFcHTpU";
        private string accessToken =
            "4678659ecbffd3567799b72c33b3fc540da9dc49c02bf65ff0b3cf29a1894055d2b1013aafa39878b130b";

        public ICollection<Comment> GetComments(int count, int offset, long postId)
        {
            var requestString = string.Format("/method/wall.getComments?" +
                                    "v=5.29&" +
                                    "extended=1&" +
                                    "count={0}&" +
                                    "offset={1}&" +
                                    "post_id={2}&" +
                                    "owner_id=-{3}&" +
                                    "access_token={4}",
                                    count,
                                    offset, 
                                    postId, 
                                    ownerId,
                                    accessToken);

            var sig = GetMD5Hash(requestString + secret);
            var url = string.Format("{0}{1}&sig={2}", domain, requestString, sig);
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var response = webClient.UploadString(url, string.Empty);
            var items = JObject.Parse(response).SelectToken("response").SelectToken("items");
            var comments = items.ToObject<IEnumerable<Comment>>().ToList();
            return comments;
        }

        public IEnumerable<Post> GetPosts(int skip, int take)
        {
            var requestString = string.Format("/method/wall.get?" +
                                    "v=5.29&" +
                                    "count={0}&" +
                                    "offset={1}&" +
                                    "filter=all&" +
                                    "domain=koko_kharkov&" +
                                    "access_token={2}",
                                    take,
                                    skip, 
                                    accessToken);

            var sig = GetMD5Hash(requestString + secret);
            var url = string.Format("{0}{1}&sig={2}", domain, requestString, sig);
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var response = webClient.DownloadString(url);
            var items = JObject
                .Parse(response)
                .SelectToken("response")
                .SelectToken("items");

            var posts = items.ToObject<IEnumerable<Post>>().ToList();
            posts.ForEach(x => x.UpdateLikesAndComments());
            return posts;
        }

        public IEnumerable<User> GetUsers(IEnumerable<int> userIds)
        {
            var requestString = string.Format("/method/users.get?" +
                        "v=5.29&" +
                        "user_ids={0}&" +
                        "fields=photo_50,screen_name", 
                        string.Join(",", userIds));

            var url = string.Format("{0}{1}", domain, requestString);
            var webClient = new WebClient { Encoding = Encoding.UTF8 };
            var response = webClient.DownloadString(url);
            var items = JObject.Parse(response).SelectToken("response");
            var users = items.ToObject<IEnumerable<User>>();
            return users;
        } 

        private static string GetMD5Hash(string input)
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
    }
}

using System.Collections.Generic;
using Chicken.Domain.Models;

namespace Chicken.Services
{
    interface IChickenService
    {
        Post GetPost(int id);

        IEnumerable<Post> GetPosts(int skip, int take);

        IEnumerable<Comment> GetComments(int id);

        void AddNewPosts();
    }
}

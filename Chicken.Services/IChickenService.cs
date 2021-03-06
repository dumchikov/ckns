﻿using System.Collections.Generic;
using Chicken.Domain.Models;

namespace Chicken.Services
{
    interface IChickenService
    {
        Post GetPost(int id);

        IEnumerable<Post> GetPosts(int skip, int take, bool withSpam = false);

        void EditPost(Post post);

        IEnumerable<Comment> GetComments(int id);

        IEnumerable<Post> AddNewPosts();
    }
}

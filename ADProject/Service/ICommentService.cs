using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADProject.Models;

namespace ADProject.Service
{
    public interface ICommentService
    {
        Task<bool> AddComment(int recipeId, Comment comment);
    }
}

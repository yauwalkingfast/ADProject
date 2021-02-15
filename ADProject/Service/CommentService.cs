using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace ADProject.Service
{
    public class CommentService : ICommentService
    {
        private readonly ADProjContext _context;

        public CommentService(ADProjContext context)
        {
            _context = context;
        }

        public async Task<bool> AddComment(int recipeId, Comment comment)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.RecipeId == recipeId);
            if(recipe == null)
            {
                return false;
            }

            comment.Recipe = recipe;
            _context.Add(comment);
            var success = await _context.SaveChangesAsync();

            return success >= 1;
        }
    }
}

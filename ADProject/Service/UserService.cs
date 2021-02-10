using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using ADProject.JsonObjects;

namespace ADProject.Service
{
    public class UserService : IUserService
    {
        private readonly ADProjContext _context;

        public UserService(ADProjContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetUserById(int? id)
        {
            ApplicationUser user = await _context.Users
                .Include(r => r.Recipes)
                .ThenInclude(r=>r.RecipeIngredients)
                .Include(r => r.Recipes)
                .ThenInclude(r=>r.RecipeSteps)
                .Include(r=>r.Recipes)
                .ThenInclude(r=>r.RecipeTags)
                .ThenInclude(r=>r.Tag)
                .Include(r => r.LikesDislikes)
                .Include(r => r.Comments)
                .Include(r => r.SavedRecipes)
                .Include(r => r.UsersGroups)
                .ThenInclude(rG => rG.Group)
                .FirstOrDefaultAsync(r => r.Id == id);

            return user;
        }

        public async Task<ApplicationUser> GetUserByUsername(string username)
        {
            ApplicationUser user = await _context.Users
                .Include(r => r.Recipes)
                .Include(r => r.LikesDislikes)
                .Include(r => r.Comments)
                .Include(r => r.UsersGroups)
                .ThenInclude(rG => rG.Group)
                .FirstOrDefaultAsync(r => r.UserName == username);

            return user;
        }

        public async Task<List<UsersGroup>> GetUserGroupByUserId(int? id)
        {
            return await _context.UsersGroups
                .Include(r => r.User)
                .Include(r => r.Group)
                .Where(r => r.UserId == id)
                .ToListAsync();
        }

        public async Task<List<UsersGroup>> GetUserGroupByGroupId(int? id)
        {
            return await _context.UsersGroups
                .Include(r => r.User)
                .Include(r => r.Group)
                .Where(r => r.GroupId == id)
                .ToListAsync();
        }

        public async Task<List<UserAllergen>> getUserAllergens(int id)
        {
            return await _context.UserAllergens
                .Include(x => x.Tag)
                .Include(x => x.User)
                .Where(x => x.UserId == id)
                .ToListAsync();
        }

        public async Task<bool> JoinGroup(UsersGroup ug)
        {
            _context.Add(ug);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }

        public async Task<bool> SaveRecipe(SaveUserRecipe saveUserRecipe)
        {
            int userId = saveUserRecipe.userId;
            int recipeId = saveUserRecipe.recipeId;

            // Check if record exist
            SavedRecipe savedRecipe = await _context.SavedRecipes
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync(x => x.RecipeId == recipeId);

            if (savedRecipe == null)
            {
                SavedRecipe saveRecipe = new SavedRecipe();
                saveRecipe.UserId = userId;
                saveRecipe.RecipeId = recipeId;

                _context.SavedRecipes.Add(saveRecipe);
                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1;
            }
            else
            {
                _context.SavedRecipes.Remove(savedRecipe);
                var saveResult = await _context.SaveChangesAsync();
                return saveResult == 1;
            }
            
        }
    }
}

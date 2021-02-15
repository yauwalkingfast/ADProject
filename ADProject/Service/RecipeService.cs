using ADProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static MoreLinq.Extensions.DistinctByExtension;

namespace ADProject.Service
{
    public class RecipeService : IRecipeService
    {
        private readonly ADProjContext _context;

        public RecipeService(ADProjContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRecipe(Recipe recipe)
        {
            ApplicationUser user = await _context.Users.FirstOrDefaultAsync(u => u.Id == recipe.UserId);
            recipe.User = user;
            recipe.RecipeTags = await this.CheckTagsDatabase(recipe.RecipeTags.ToList());
            recipe.RecipeGroups = await this.CheckGroupDatabase(recipe.RecipeGroups.ToList());

            _context.Add(recipe);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }

        public async Task<bool> AddRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            _context.Add(recipeIngredient);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> AddRecipeSteps(RecipeStep recipeStep)
        {
            _context.Add(recipeStep);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }


        public async Task<bool> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.User)
                //.Include(r => r.RecipeSteps)
                //.Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(m => m.RecipeId == id);

            _context.Recipes.Remove(recipe);
            var saveResult = await _context.SaveChangesAsync();
            return saveResult >= 1;
        }


        public void AddRecipeNonAsync(Recipe recipe)
        {
            _context.Add(recipe);
            _context.SaveChanges();
        }

        public Recipe FindRecipeById(int id)
        {
            Recipe recipe = _context.Recipes.Where(
                x => x.RecipeId == id).SingleOrDefault();

            return recipe;
        }


        public List<RecipeIngredient> FindRecipeStepsByRecipeId(int id)
        {
            List<RecipeIngredient> recipeIngredients = _context.RecipeIngredients.Where(
                x => x.Recipe.RecipeId == id).ToList();

            return recipeIngredients;
        }

        public async Task<bool> EditRecipe(int id, Recipe recipe)
        {
            try
            {
                var dbRecipe = await _context.Recipes
                    .Include(r => r.RecipeSteps)
                    .Include(r => r.RecipeIngredients)
                    .Include(r => r.RecipeTags)
                    .ThenInclude(rtag => rtag.Tag)
                    .Include(r => r.RecipeGroups)
                    .ThenInclude(rgroup => rgroup.Group)
                    .FirstOrDefaultAsync(r => r.RecipeId == id);

                _context.RecipeIngredients.RemoveRange(dbRecipe.RecipeIngredients);
                _context.RecipeSteps.RemoveRange(dbRecipe.RecipeSteps);
                _context.RecipeTags.RemoveRange(dbRecipe.RecipeTags);
                _context.RecipeGroups.RemoveRange(dbRecipe.RecipeGroups);

                dbRecipe.Title = recipe.Title;
                dbRecipe.Description = recipe.Description;
                dbRecipe.DurationInMins = recipe.DurationInMins;
                dbRecipe.Calories = recipe.Calories;
                dbRecipe.ServingSize = recipe.ServingSize;
                dbRecipe.IsPublished = recipe.IsPublished;
                dbRecipe.MainMediaUrl = recipe.MainMediaUrl;
                dbRecipe.RecipeIngredients = recipe.RecipeIngredients;
                dbRecipe.RecipeSteps = recipe.RecipeSteps;

                dbRecipe.RecipeTags = await this.CheckTagsDatabase(recipe.RecipeTags.ToList());
                dbRecipe.RecipeGroups = await this.CheckGroupDatabase(recipe.RecipeGroups.ToList());

                await _context.SaveChangesAsync();
                return true;
            } catch (DbUpdateConcurrencyException)
            {
                return false;
            }

        }

        public async Task<List<Recipe>> GetAllRecipes()
        {
            return await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Include(r => r.SavedRecipes)
                .ThenInclude(sr => sr.User)
                .ToListAsync();
        }

        public async Task<IQueryable<Recipe>> GetAllRecipesQueryable()
        {
            var recipes = await this.GetAllRecipes();
            return recipes.AsQueryable();
        }

        public async Task<List<Recipe>> GetAllRecipesBasic()
        {
            List<Recipe> rList = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .ToListAsync();

            foreach (Recipe r in rList)
            {
                ApplicationUser n = new ApplicationUser
                {

                    UserName = r.User.UserName
                };

                r.User = n;
            }

            return rList;
        }

        public async Task<List<Recipe>> GetAllRecipesSearch(string search)
        {
            List<Recipe> rList = await _context.Recipes
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Where(r => r.Title.Contains(search) ||
                          r.Description.Contains(search) ||
                          r.RecipeIngredients.Any(y => y.Ingredient.Contains(search)) ||
                          r.RecipeTags.Any(y => y.Tag.TagName.Contains(search)))
                .ToListAsync();

/*            foreach (Recipe r in rList)
            {
                ApplicationUser n = new ApplicationUser
                {

                    UserName = r.User.UserName
                };

                r.User = n;
            }*/

            return rList;
        }

        public async Task<IQueryable<Recipe>> GetAllRecipeSearchQueryable(string search)
        {
            var searchedRecipeList = await this.GetAllRecipesSearch(search);
            return searchedRecipeList.AsQueryable();
        }

        public async Task<Recipe> GetRecipeById(int? id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .ThenInclude(c => c.User)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Include(r => r.RecipeGroups)
                .ThenInclude(rgroup => rgroup.Group)
                .Include(r => r.SavedRecipes)
                .ThenInclude(sr => sr.User)
                .FirstOrDefaultAsync(r => r.RecipeId == id);

            recipe.RecipeSteps.Sort((x, y) => x.StepNumber.CompareTo(y.StepNumber));

            return recipe;
        }

        // Check if the group exist
        private async Task<List<RecipeGroup>> CheckGroupDatabase(List<RecipeGroup> recipeGroups)
        {
            List<RecipeGroup> foundGroups = new List<RecipeGroup>();
            for (int i = 0; i < recipeGroups.Count; i++)
            {
                var existGroup = await _context.Groups.FirstOrDefaultAsync(global => global.GroupName.ToLower() == recipeGroups[i].Group.GroupName.ToLower().Trim());
                if (existGroup != null)
                {
                    foundGroups.Add(new RecipeGroup
                    {
                        GroupId = existGroup.GroupId,
                        Group = existGroup
                    });
                }
            }

            return foundGroups.DistinctBy(rg => rg.GroupId).ToList();
        }

        // Check is the same tag already exist in the database
        private async Task<List<RecipeTag>> CheckTagsDatabase(List<RecipeTag> recipeTag)
        {
            for (int i = 0; i < recipeTag.Count(); i++)
            {
                var existTag = await _context.Tags.FirstOrDefaultAsync(t => t.TagName.ToLower() == recipeTag[i].Tag.TagName.ToLower().Trim());
                if (existTag != null)
                {
                    recipeTag[i].TagId = existTag.TagId;
                    recipeTag[i].Tag = existTag;
                }
            }
            return recipeTag.DistinctBy(rt => rt.Tag.TagName).ToList();
        }

        public async Task<List<Recipe>> GetAllRecipesByUserId(int? id)
        {
            return await _context.Recipes

                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Where(r => r.UserId == id)
                .ToListAsync();
        }

        public async Task<IQueryable<Recipe>> GetAllRecipesByUserIdQueryable(int? id)
        {
            var recipes = await this.GetAllRecipesByUserId(id);
            return recipes.AsQueryable();
        }

        public async Task<List<Recipe>> SearchMyRecipe(String search, int? id)
        {
            return await _context.Recipes
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Where(r => r.UserId == id)
                .Where(r => r.Title.Contains(search) ||
                          r.Description.Contains(search) ||
                          r.RecipeIngredients.Any(y => y.Ingredient.Contains(search)) ||
                          r.RecipeTags.Any(y => y.Tag.TagName.Contains(search)))
                .ToListAsync();
        }

        public async Task<IQueryable<Recipe>> SearchMyRecipeQueryable(String search, int? id)
        {
            var recipes = await this.SearchMyRecipe(search, id);
            return recipes.AsQueryable();
        }

        public async Task<bool> SaveRecipe(int recipeId, string username)
        {
            var recipe = await this.GetRecipeById(recipeId);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            var savedRecipes = await _context.SavedRecipes.Where(sr => sr.User.UserName == username).ToListAsync();

            if (recipe == null || user == null || recipe.User.Id == user.Id || savedRecipes.Any(sr => sr.RecipeId == recipe.RecipeId))
            {
                return false;
            }

            _context.Add(new SavedRecipe
            {
                UserId = user.Id,
                RecipeId = recipe.RecipeId,
                Recipe = recipe,
                User = user
            });

            var saveresult =  await _context.SaveChangesAsync();
            return saveresult >= 1;
        }

        public async Task<bool> RemoveRecipe(int recipeId, string username)
        {
            var savedRecipe = await _context.SavedRecipes.FirstOrDefaultAsync(sr => sr.RecipeId == recipeId && sr.User.UserName == username);
            if(savedRecipe == null)
            {
                return false;
            }

             _context.SavedRecipes.Remove(savedRecipe);
            var successresult = await _context.SaveChangesAsync();
            return successresult >= 1;
        }

        public async Task<IQueryable<SavedRecipe>> AllSavedRecipes(string username)
        {
            return _context.SavedRecipes
                    .Include(sr => sr.Recipe)
                    .ThenInclude(r => r.RecipeTags)
                    .ThenInclude(rt => rt.Tag)
                    .Include(sr => sr.User)
                    .Where(sr => sr.User.UserName == username)
                    .AsQueryable();
        }

        public async Task<IQueryable<SavedRecipe>> SearchSavedRecipes(string username, string search)
        {
            var myGroup = await this.AllSavedRecipes(username);
            return myGroup.Where(sr => sr.Recipe.Title.Contains(search) ||
                      sr.Recipe.Description.Contains(search) ||
                      sr.Recipe.RecipeIngredients.Any(y => y.Ingredient.Contains(search)) ||
                      sr.Recipe.RecipeTags.Any(y => y.Tag.TagName.Contains(search)))
                .AsQueryable();  
        }
    }
}

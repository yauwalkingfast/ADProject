using ADProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return saveResult == 1;
        }


        public void AddRecipeNonAsync (Recipe recipe)
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
            } catch(DbUpdateConcurrencyException)
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
                .ToListAsync();
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
                User n = new User
                {
                    UserId = r.User.UserId,
                    Username = r.User.Username
                };

                r.User = n;
            }

            return rList;
        }

        public async Task<List<Recipe>> GetAllRecipesSearch(string search)
        {
            List<Recipe> rList = await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Where(r => r.Title.Contains(search)
                            || r.Description.Contains(search)
                            || r.RecipeIngredients.Any(y => y.Ingredient.Contains(search))
                            || r.RecipeTags.Any(y => y.Tag.TagName.Contains(search)))
                .ToListAsync();

            foreach (Recipe r in rList)
            {
                User n = new User
                {
                    UserId = r.User.UserId,
                    Username = r.User.Username
                };

                r.User = n;
            }

            return rList;
        }


        public async Task<Recipe> GetRecipeById(int? id)
        {
            return await _context.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeSteps)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.Comments)
                .Include(r => r.LikesDislikes)
                .Include(r => r.RecipeTags)
                .ThenInclude(rtag => rtag.Tag)
                .Include(r => r.RecipeGroups)
                .ThenInclude(rgroup => rgroup.Group)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }
        
        // Check if the group exist
        private async Task<List<RecipeGroup>> CheckGroupDatabase(List<RecipeGroup> recipeGroups)
        {
            List<RecipeGroup> foundGroups = new List<RecipeGroup>();
            for(int i = 0; i < recipeGroups.Count; i++)
            {
                var existGroup = await _context.Groups.FirstOrDefaultAsync(global => global.GroupName.ToLower() == recipeGroups[i].Group.GroupName.ToLower().Trim());
                if(existGroup != null)
                {
                    foundGroups.Add(new RecipeGroup
                    {
                        GroupId = existGroup.GroupId,
                        Group = existGroup
                    });
                }
            }

            return foundGroups;
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
            return recipeTag;
        }
    }
}

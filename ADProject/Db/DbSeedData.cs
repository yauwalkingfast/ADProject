using ADProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.DbSeeder
{
    public class DbSeedData
    {
        private readonly ADProjContext db;

        public DbSeedData(ADProjContext db)
        {
            this.db = db;
        }

        public void Init()
        {
            AddRecipes();
        }

        protected void AddRecipes()
        {
            db.Users.Add(new User
            {
                FirstName = "Wilson",
                LastName = "Chan",
                Username = "wc",
                Password = "12345",
                Email = "wilson@email.com",
                IsAdmin = true
            });

            db.SaveChanges();

            User user = db.Users.FirstOrDefault();

            List<RecipeIngredient> recipeIngredient = new List<RecipeIngredient>();
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "Chocolate",
                Quantity = 100,
                UnitOfMeasurement = "grams"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "Milk",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            List<RecipeStep> recipeSteps = new List<RecipeStep>();
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 1,
                TextInstructions = "mix chocolate with milk",
                MediaFileUrl = "step 1 image"
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 2,
                TextInstructions = "put in oven",
                MediaFileUrl = "step 2 image"
            });

            db.Recipes.Add(new Recipe
            {
                UserId = user.UserId,
                Title = "Chocolate Cake",
                Description = "Sweets and Tasty chocolate cake",
                DateCreated = new DateTime(2008, 5, 1, 8, 30, 52),
                DurationInMins = 60,
                Calories = 500,
                IsPublished = true,
                MainMediaUrl = "some image url",
                RecipeIngredients = recipeIngredient,
                RecipeSteps = recipeSteps,
            });

            db.SaveChanges();
        }
    }
}

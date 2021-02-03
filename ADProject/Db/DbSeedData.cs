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

            //Please uncomment the following 2 ingredients after testing is over

            /*recipeIngredient.Add(new RecipeIngredient
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
            });*/

            //To test the generate allergen tags API connection; remove later. 
            //Also, please change the ingredient string limit back to 20 from 100 after testing is over. 
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "4 large eggs, room temperature",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "2 cups sugar",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "1 teaspoon vanilla extract",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "2-1/4 cups all purpose flour",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "2-1/4 teaspoons baking powder",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "1-1/4 cups 2% milk",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "10 tablespoons butter, cubed",
                Quantity = 100,
                UnitOfMeasurement = "ml"
            });

            List<RecipeStep> recipeSteps = new List<RecipeStep>();
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 1,
                TextInstructions = "mix chocolate with milk",
                MediaFileUrl = "https://www.wikihow.com/images/thumb/f/ff/Make-a-Simple-Chocolate-Cake-Step-3-Version-3.jpg/aid1334248-v4-728px-Make-a-Simple-Chocolate-Cake-Step-3-Version-3.jpg"
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 2,
                TextInstructions = "put in oven",
                MediaFileUrl = "https://www.deliciousmagazine.co.uk/wp-content/uploads/2018/09/321197-1-eng-GB_4469.jpg"
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
                MainMediaUrl = "https://th.bing.com/th/id/OIP.P70fg98tIgi-8b-pMMhZXAHaFj?pid=Api&rs=1",
                RecipeIngredients = recipeIngredient,
                RecipeSteps = recipeSteps,
            });

            db.SaveChanges();
        }
    }
}

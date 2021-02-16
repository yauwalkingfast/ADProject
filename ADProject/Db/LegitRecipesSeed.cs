using ADProject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ADProject.Db
{
    public class LegitRecipesSeed
    {
        private readonly ADProjContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LegitRecipesSeed(ADProjContext db, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this._userManager = _userManager;
        }

        public void Init()
        {
            AddTags();
            AddYK();
        }

        public void AddYK()
        {
            ApplicationUser u1 = new ApplicationUser
            {
                FirstName = "Yuen Kwan",
                LastName = "Chia",
                UserName = "yk",
                NormalizedUserName = "yk".ToUpper(),
                Email = "yk@email.com",
                NormalizedEmail = "yk@email.com".ToUpper(),
                IsAdmin = true,
            };

            u1.PasswordHash = _userManager.PasswordHasher.HashPassword(u1, "12345");
            u1.SecurityStamp = Guid.NewGuid().ToString();
            db.Users.Add(u1);
            db.SaveChanges();

            // Recipe 1
            List<RecipeIngredient> recipeIngredient = new List<RecipeIngredient>();
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "plain flour",
                Quantity = 140,
                UnitOfMeasurement = "g"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "ground almonds",
                Quantity = 140,
                UnitOfMeasurement = "g"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "icing sugar",
                Quantity = 60,
                UnitOfMeasurement = "g"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "cornstarch",
                Quantity = 20,
                UnitOfMeasurement = "g"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "salt",
                Quantity = 0.25,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "baking soda",
                Quantity = 0.5,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "egg york",
                Quantity = 1
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "virgin coconut oil",
                Quantity = 90,
                UnitOfMeasurement = "g"
            });
            recipeIngredient.Add(new RecipeIngredient
            {
                Ingredient = "flaked almonds",
                Quantity = 45,
                UnitOfMeasurement = "g"
            });

            List<RecipeStep> recipeSteps = new List<RecipeStep>();
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 1,
                MediaFileUrl = "https://asianfoodnetwork.com/content/dam/afn/global/en/recipes/almond-cookies/AFN_almond_cookies_step1_1.jpg",
                TextInstructions = "Preheat oven to 150°C. Line two baking trays with baking paper."
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 2,
                MediaFileUrl = "https://asianfoodnetwork.com/content/dam/afn/global/en/recipes/almond-cookies/AFN_almond_cookies_step2.jpg",
                TextInstructions = "Place the plain flour, ground almonds, icing sugar, cornstarch, salt, baking soda and baking powder into a food processor. Whizz to combine."
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 3,
                TextInstructions = "Beat the egg yolk and oil together, and pour into the food processor. Pulse until the dough has come together. If it’s still too crumbly, add a little more oil."
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 4,
                TextInstructions = "Tip the dough out onto your work surface. Knead it lightly once or twice, then roll out to a 1 cm thickness.",
                MediaFileUrl = "https://asianfoodnetwork.com/content/dam/afn/global/en/recipes/almond-cookies/AFN_almond_cookies_step3.jpg"
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 5,
                TextInstructions = "Using a small round cookie cutter, cut out the cookies and place them on the lined baking trays."
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 6,
                TextInstructions = "Brush each cookie with some egg wash, and top with a flaked almond."
            });
            recipeSteps.Add(new RecipeStep
            {
                StepNumber = 7,
                TextInstructions = "To pack, keep cookies in air-tight containers to maintain their freshness.",
                MediaFileUrl = "https://asianfoodnetwork.com/content/dam/afn/global/en/recipes/almond-cookies/AFN_almond_cookies_main_image.jpg"
            });

            Tag t1 = new Tag
            {
                TagName = "Chinese",
                Warning = ""
            };
            Tag t2 = new Tag
            {
                TagName = "Egg",
                Warning = ""
            };
            Tag t3 = new Tag
            {
                TagName = "Chinese New Year",
                Warning = ""
            };
            Tag t4 = new Tag
            {
                TagName = "Desert",
                Warning = ""
            };
            Tag t5 = new Tag
            {
                TagName = "Flour",
                Warning = ""
            };
            Tag t6 = new Tag
            {
                TagName = "Nuts",
                Warning = "Nuts"
            };

            db.Add(t1);
            db.Add(t2);
            db.Add(t3);
            db.Add(t4);
            db.Add(t5);
            db.Add(t6);

            db.SaveChanges();

            List<RecipeTag> recipeTags = new List<RecipeTag>();
            recipeTags.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t1
            });
            recipeTags.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t2
            });
            recipeTags.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t3
            });
            recipeTags.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t4
            });
            recipeTags.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t5
            });
            recipeTags.Add(new RecipeTag
            {
                IsAllergenTag = true,
                Tag = t6
            });

            db.Add(new Recipe
            {
                Title = "Almond Cookies",
                Description = "Treat your guests to a batch of these buttery Almond Cookies this Chinese New Year. With a crumbly texture that simply melts in your mouth, this sweet treat is definitely a crowd favorite and will be wiped out in no time. Good thing it’s super easy to bake! In under an hour, this well-loved cookie will be ready for you and your loved ones to devour, fresh out of the oven.",
                DurationInMins = 60,
                Calories = 500,
                ServingSize = 2,
                IsPublished = true,
                MainMediaUrl = "https://asianfoodnetwork.com/content/dam/afn/global/en/recipes/almond-cookies/AFN_almond_cookies_main_image.jpg",
                RecipeIngredients = recipeIngredient,
                RecipeSteps = recipeSteps,
                RecipeTags = recipeTags,
                User = u1,
                DateCreated = DateTime.Now
            });

            db.SaveChanges();
        }

        protected void AddTags()
        {
            string currentDir = Directory.GetCurrentDirectory();

            using (var reader = new StreamReader(Path.Combine(currentDir, "Db", "FoodData.csv")))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var data = line.Split(',');

                    db.Tags.Add(new Tag
                    {
                        TagName = data[3],
                        Warning = data[4]
                    });
                }
            }

            db.SaveChanges();
        }

    }
}

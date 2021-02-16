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

            // Recipe 2
            List<RecipeIngredient> recipeIngredient2 = new List<RecipeIngredient>();
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "beef chunk roast",
                Quantity = 1,
                UnitOfMeasurement = "2-lb"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "dried oregano",
                Quantity = 2,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "brown sugar",
                Quantity = 2,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "kosher salt",
                Quantity = 0.5,
                UnitOfMeasurement = ""
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "chili powder",
                Quantity = 1,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "cumin",
                Quantity = 1,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "garlic powder",
                Quantity = 1,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "vegetable oil",
                Quantity = 1,
                UnitOfMeasurement = "tsp"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "medium yellow onion, sliced",
                Quantity = 1,
                UnitOfMeasurement = ""
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "Mexican beer",
                Quantity = 12,
                UnitOfMeasurement = "oz"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "bag corn chips",
                Quantity = 12,
                UnitOfMeasurement = "oz"
            });
            recipeIngredient2.Add(new RecipeIngredient
            {
                Ingredient = "Monterey Jack cheese",
                Quantity = 3,
                UnitOfMeasurement = "slices"
            });

            List<RecipeStep> recipeSteps2 = new List<RecipeStep>();
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 1,
                MediaFileUrl = "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/190423-instant-pot-nachos-159-1556728757.jpg?crop=1.00xw:0.376xh;0,0.120xh&resize=768:*",
                TextInstructions = "In a small bowl, whisk to combine oregano, brown sugar, salt, chili powder, cumin, and garlic powder. Rub spice mix all over roast."
            });
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 2,
                TextInstructions = "Heat Instant Pot to Sauté and add vegetable oil. Sear all sides of chuck roast until golden, about 2 minutes per side. Remove roast."
            });
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 3,
                TextInstructions = "Pour beer into Instant Pot, then add chuck roast back to pot. Scatter onions over pot roast and secure Instant Pot lid."
            });
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 4,
                TextInstructions = "Select Pressure Cook and cook on high for 2 1/2 hours. Let pressure release naturally for 10 minutes, then quick release remaining air. Remove roast from instant pot and use two forks to shred into bite-sized pieces."
            });
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 5,
                TextInstructions = "Preheat oven to 375and line a large baking sheet with aluminum foil. Spread an even layer of chips onto the baking sheet, then top with 1/3 of the cheese, jalapenos, and shredded beef."
            });
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 6,
                TextInstructions = " Top with more chips, and another 1/3 of cheese, jalapeños, and beef. Finish with one more layer of chips and the remaining cheese, jalapeños, and beef. Bake until cheese is melty and chips have crisped slightly, 10 minutes.",
            });
            recipeSteps2.Add(new RecipeStep
            {
                StepNumber = 7,
                TextInstructions = "Garnish with avocado, radishes, cilantro, and red onion. Serve with lime wedges on the side for squeezing."
            });

            Tag t7 = new Tag
            {
                TagName = "Cheese",
                Warning = "Lactose Intolerant"
            };
            Tag t8 = new Tag
            {
                TagName = "Beef",
                Warning = ""
            };

            db.Add(t7);
            db.Add(t8);
            db.SaveChanges();

            List<RecipeTag> recipeTags2 = new List<RecipeTag>();
            recipeTags2.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t7
            });
            recipeTags2.Add(new RecipeTag
            {
                IsAllergenTag = true,
                Tag = t7
            });
            recipeTags2.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = t8
            });

            db.Add(new Recipe
            {
                Title = "Instant Pot Shredded Beef Nachos",
                Description = "Who doesn't love a big tray of nachos? These are particularly good, considering they're loaded with delicious, tender braised beef chuck roast. If your cut of meat is especially big, you can save the leftovers for quick and easy tacos—just add tortillas!",
                DurationInMins = 60,
                Calories = 500,
                ServingSize = 2,
                IsPublished = true,
                MainMediaUrl = "https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/190423-instant-pot-nachos-191-1556728757.jpg?crop=0.668xw:1.00xh;0.247xw,0.00255xh&resize=980:*",
                RecipeIngredients = recipeIngredient2,
                RecipeSteps = recipeSteps2,
                RecipeTags = recipeTags2,
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

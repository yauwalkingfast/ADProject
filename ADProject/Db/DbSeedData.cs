using ADProject.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace ADProject.DbSeeder
{
    public class DbSeedData
    {
        private readonly ADProjContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        public DbSeedData(ADProjContext db, UserManager<ApplicationUser> _userManager)
        {
            this.db = db;
            this._userManager = _userManager;
        }

        public void Init()
        {
            AddRecipes();
            AddTags();
            AddGroups();
            AddUsers();

            // Use this version to test authentication
            AddUsersVer2();
            AddRecipesVer2();
            AddGroupsVer2();
        }

        protected void AddGroupsVer2()
        {
            ApplicationUser user1 = db.Users.FirstOrDefault(user => user.NormalizedUserName.Equals("AK"));
            ApplicationUser user2 = db.Users.FirstOrDefault(user => user.NormalizedUserName.Equals("HM"));

            List<UsersGroup> ug = new List<UsersGroup>();
            ug.Add(new UsersGroup
            {
                UserId = user1.Id,
                IsMod = true,
                User = user1
            });
            ug.Add(new UsersGroup
            {
                UserId = user2.Id,
                IsMod = false,
                User = user2
            });

            db.Groups.Add(new Group
            {
                GroupName = "Hololive Official",
                GroupPhoto = "https://user-images.strikinglycdn.com/res/hrscywv4p/image/upload/c_limit,fl_lossy,h_9000,w_1200,f_auto,q_auto/1369026/124086_244660.png",
                Description = "Official hololive group",
                DateCreated = new DateTime(2008, 5, 1, 8, 30, 52),
                IsPublished = true,
                UsersGroups = ug
            });

            db.SaveChanges();
        }

        protected void AddRecipesVer2()
        {
            ApplicationUser u1 = new ApplicationUser
            {
                FirstName = "Akai",
                LastName = "Haato",
                UserName = "ak",
                NormalizedUserName = "ak".ToUpper(),
                Email = "ak@email.com",
                NormalizedEmail = "ak@email.com".ToUpper(),
                IsAdmin = true,
            };

            u1.PasswordHash = _userManager.PasswordHasher.HashPassword(u1, "12345");
            u1.SecurityStamp = _userManager.CreateSecurityTokenAsync(u1).ToString();

            db.Users.Add(u1);
            db.SaveChanges();

            ApplicationUser user = db.Users.FirstOrDefault(user => user.NormalizedUserName.Equals("AK"));

            List<RecipeIngredient> recipeIngredient = new List<RecipeIngredient>();

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

            List<RecipeTag> recipeTag = new List<RecipeTag>();
            recipeTag.Add(new RecipeTag
            {
                IsAllergenTag = true,
                Tag = new Tag
                {
                    TagName = "Milk",
                    Warning = "Lactose Intolerence"
                }
            });
            recipeTag.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = new Tag
                {
                    TagName = "Cake",
                    Warning = ""
                }
            });
            recipeTag.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = new Tag
                {
                    TagName = "Dessert",
                    Warning = ""
                }
            });

            db.Recipes.Add(new Recipe
            {
                UserId = user.Id,
                Title = "I am lazy",
                Description = "So I just copy paste",
                DateCreated = new DateTime(2008, 5, 1, 8, 30, 52),
                DurationInMins = 60,
                Calories = 500,
                IsPublished = true,
                MainMediaUrl = "https://th.bing.com/th/id/OIP.P70fg98tIgi-8b-pMMhZXAHaFj?pid=Api&rs=1",
                RecipeIngredients = recipeIngredient,
                RecipeSteps = recipeSteps,
                RecipeTags = recipeTag
            });

            db.SaveChanges();
        }

        protected void AddUsersVer2()
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
            u1.SecurityStamp = _userManager.CreateSecurityTokenAsync(u1).ToString();
            db.Users.Add(u1);

            ApplicationUser u2 = new ApplicationUser
            {
                FirstName = "Amelia",
                LastName = "Watson",
                UserName = "aw",
                NormalizedUserName = "aw".ToUpper(),
                Email = "aw@email.com",
                NormalizedEmail = "aw@email.com".ToUpper(),
                IsAdmin = true,
            };

            u2.PasswordHash = _userManager.PasswordHasher.HashPassword(u2, "12345");
            u2.SecurityStamp = _userManager.CreateSecurityTokenAsync(u2).ToString();
            db.Users.Add(u2);

            ApplicationUser u3 = new ApplicationUser
            {
                FirstName = "Honshou",
                LastName = "Marine",
                UserName = "hm",
                NormalizedUserName = "hm".ToUpper(),
                Email = "hm@email.com",
                NormalizedEmail = "hm@email.com".ToUpper(),
                IsAdmin = true,
            };

            u3.PasswordHash = _userManager.PasswordHasher.HashPassword(u3, "12345");
            u3.SecurityStamp = _userManager.CreateSecurityTokenAsync(u3).ToString();

            db.Users.Add(u3);

            db.SaveChanges();
        }

        protected void AddUsers()
        {
            db.Users.Add(new ApplicationUser
            {
                FirstName = "Jackie",
                LastName = "Chan",
                UserName = "jc",
                NormalizedUserName = "jc".ToUpper(),
                NormalizedEmail = "jackie@email.com".ToUpper(),
                PasswordHash = BC.HashPassword("12345"),
                Email = "jackie@email.com",
                IsAdmin = true
            });

            db.Users.Add(new ApplicationUser
            {
                FirstName = "Chun Sing",
                LastName = "Chan",
                UserName = "ccs",
                NormalizedUserName = "ccs".ToUpper(),
                NormalizedEmail = "ccs@email.com".ToUpper(),
                PasswordHash = BC.HashPassword("12345"),
                Email = "ccs@email.com",
                IsAdmin = true
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

        protected void AddRecipes()
        {
            db.Users.Add(new ApplicationUser
            {
                FirstName = "Wilson",
                LastName = "Chan",
                UserName = "wc",
                NormalizedUserName = "wc".ToUpper(),
                PasswordHash = BC.HashPassword("12345"),
                Email = "wilson@email.com",
                NormalizedEmail = "wilson@email.com".ToUpper(),
                IsAdmin = true
            });

            db.SaveChanges();

            ApplicationUser user = db.Users.FirstOrDefault();

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

            List<RecipeTag> recipeTag = new List<RecipeTag>();
            recipeTag.Add(new RecipeTag 
            { 
                IsAllergenTag = true, 
                Tag = new Tag 
                { 
                    TagName = "Milk", 
                    Warning = "Lactose Intolerence" 
                } 
            });
            recipeTag.Add(new RecipeTag 
            { 
                IsAllergenTag = false, 
                Tag = new Tag 
                { 
                    TagName = "Cake", 
                    Warning = "" 
                } 
            });
            recipeTag.Add(new RecipeTag
            {
                IsAllergenTag = false,
                Tag = new Tag
                {
                    TagName = "Dessert",
                    Warning = ""          
                }
            });

            db.Recipes.Add(new Recipe
            {
                UserId = user.Id,
                Title = "Chocolate Cake",
                Description = "Sweets and Tasty chocolate cake",
                DateCreated = new DateTime(2008, 5, 1, 8, 30, 52),
                DurationInMins = 60,
                Calories = 500,
                IsPublished = true,
                MainMediaUrl = "https://th.bing.com/th/id/OIP.P70fg98tIgi-8b-pMMhZXAHaFj?pid=Api&rs=1",
                RecipeIngredients = recipeIngredient,
                RecipeSteps = recipeSteps,
                RecipeTags= recipeTag
            });

            db.SaveChanges();

            db.Recipes.Add(new Recipe
            {
                UserId = user.Id,
                Title = "Mala Squid",
                Description = "Hot and spicy",
                DateCreated = new DateTime(2010, 10, 1, 8, 30, 52),
                DurationInMins = 30,
                Calories = 600,
                IsPublished = true,
                MainMediaUrl = "https://singaporebeauty.com/wp-content/uploads/2017/10/dm-chicken-squid-close-up.jpg",
                //RecipeIngredients = recipeIngredient,
                //RecipeSteps = recipeSteps,
                //RecipeTags = recipeTag
            });

            db.SaveChanges();
        }

        protected void AddGroups()
        {
            db.Users.Add(new ApplicationUser
            {
                FirstName = "Tingkai",
                LastName = "Chua",
                UserName = "ctk",
                NormalizedEmail = "ctk@email.com".ToUpper(),
                NormalizedUserName = "ctk".ToUpper(),
                PasswordHash = BC.HashPassword("12345"),
                Email = "ctk@email.com",
                IsAdmin = true
            });

            db.SaveChanges();

            db.Groups.Add(new Group
            {
                GroupName = "Hololive Fans",
                GroupPhoto = "https://user-images.strikinglycdn.com/res/hrscywv4p/image/upload/c_limit,fl_lossy,h_9000,w_1200,f_auto,q_auto/1369026/124086_244660.png",
                Description = "For all hololive fans",
                DateCreated = new DateTime(2008, 5, 1, 8, 30, 52),
                IsPublished = true
            });

            db.Groups.Add(new Group
            {
                GroupName = "Esther's fan club",
                GroupPhoto = "somephoto",
                Description = "Yuen Kwan is her no.1 fan",
                DateCreated = new DateTime(2018, 5, 1, 8, 30, 52),
                IsPublished = true
            });

            db.Groups.Add(new Group
            {
                GroupName = "test",
                GroupPhoto = "somephoto",
                Description = "test",
                DateCreated = new DateTime(2018, 5, 1, 8, 30, 52),
                IsPublished = true
            });

            db.Groups.Add(new Group
            {
                GroupName = "No Ramen No Life",
                GroupPhoto = "https://www.justonecookbook.com/wp-content/uploads/2017/07/Spicy-Shoyu-Ramen-NEW-500x400.jpg",
                Description = "Shoyu, Tonkotsu and Shio is our holy trinity",
                DateCreated = new DateTime(2008, 5, 15, 8, 30, 52),
                IsPublished = true

            });

            db.Groups.Add(new Group
            {
                GroupName = "Curry and Spices",
                GroupPhoto = "https://d3e8d6e8.rocketcdn.me/wp-content/uploads/2018/11/South-Indian-Chicken-Curry-3-of-5.jpg",
                Description = "Let the aroma soak up our senses",
                DateCreated = new DateTime(2010, 5, 15, 8, 30, 52),
                IsPublished = true

            });

            db.Groups.Add(new Group
            {
                GroupName = "Korean Cuisine",
                GroupPhoto = "https://christieathome.com/wp-content/uploads/2020/12/Jajangmyeon3-scaled.jpg",
                Description = "Oppa Saranghae",
                DateCreated = new DateTime(2012, 5, 15, 8, 30, 52),
                IsPublished = true

            });

            db.SaveChanges();

            ApplicationUser user = db.Users.FirstOrDefault();
            Group group = db.Groups.FirstOrDefault();
            Group group2 = db.Groups.Where(x => x.GroupId == 2).FirstOrDefault();

            db.UsersGroups.Add(new UsersGroup
            {
                GroupId = group.GroupId,
                UserId = user.UserId,
                IsMod = true
            });

            db.UsersGroups.Add(new UsersGroup
            {
                GroupId = group2.GroupId,
                UserId = user.UserId,
                IsMod = true
            });

            db.UsersGroups.Add(new UsersGroup
            {
                GroupId = 3,
                UserId = user.UserId,
                IsMod = true
            });

            db.SaveChanges();

            db.RecipeGroups.Add(new RecipeGroup
            {
                GroupId = 4,
                RecipeId = 1
            });

            db.RecipeGroups.Add(new RecipeGroup
            {
                GroupId = 4,
                RecipeId = 2
            });

            db.SaveChanges();


        }
    }
}

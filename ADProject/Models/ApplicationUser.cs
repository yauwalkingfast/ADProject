using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace ADProject.Models
{
    [JsonObject]
    public class ApplicationUser:IdentityUser<int>
                    
    {
        public ApplicationUser()
        {
            Comments = new List<Comment>();
            FollowUserFollowedUsers = new List<FollowUser>();
            FollowUserFollowingUsers = new List<FollowUser>();
            LikesDislikes = new List<LikesDislike>();
            Recipes = new List<Recipe>();
            SavedRecipes = new List<SavedRecipe>();
            UserAllergens = new List<UserAllergen>();
            UsersGroups = new List<UsersGroup>();
        }

        [NotMapped]
        public int UserId { get { return Id; } }

        /*[Required]*/
        [Column("firstName")]
        [StringLength(30)]
        public string FirstName { get; set; }

        /*[Required]*/
        [Column("lastName")]
        [StringLength(30)]
        public string LastName { get; set; }
       
        public bool? IsAdmin { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(Comment.User))]
        public virtual IEnumerable<Comment> Comments { get; set; }
        [InverseProperty(nameof(FollowUser.FollowedUser))]
        public virtual IEnumerable<FollowUser> FollowUserFollowedUsers { get; set; }
        [InverseProperty(nameof(FollowUser.FollowingUser))]
        public virtual IEnumerable<FollowUser> FollowUserFollowingUsers { get; set; }
        [InverseProperty(nameof(LikesDislike.User))]

        [JsonIgnore]
        public virtual IEnumerable<LikesDislike> LikesDislikes { get; set; }
        [InverseProperty(nameof(Recipe.User))]

        [JsonIgnore]
        public virtual IEnumerable<Recipe> Recipes { get; set; }
        [InverseProperty(nameof(SavedRecipe.User))]
        public virtual IEnumerable<SavedRecipe> SavedRecipes { get; set; }
        [InverseProperty(nameof(UserAllergen.User))]
        public virtual IEnumerable<UserAllergen> UserAllergens { get; set; }
        [InverseProperty(nameof(UsersGroup.User))]
        public virtual IEnumerable<UsersGroup> UsersGroups { get; set; }

        
    }
}

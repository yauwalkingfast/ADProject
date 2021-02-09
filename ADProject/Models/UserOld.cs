using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

#nullable disable

namespace ADProject.Models
{
    [JsonObject]
    [Table("User")]
    public partial class UserOld { 
        public UserOld()
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

        [Key]
        public int UserId { get; set; }
        [Required]
        [Column("firstName")]
        [StringLength(30)]
        public string FirstName { get; set; }
        [Required]
        [Column("lastName")]
        [StringLength(30)]
        public string LastName { get; set; }
        [Required]
        [Column("username")]
        [StringLength(30)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(30)]
        public string Password { get; set; }
        [Required]
        [Column("email")]
        [StringLength(30)]
        public string Email { get; set; }
        [Column("isAdmin")]
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

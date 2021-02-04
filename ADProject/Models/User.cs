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
    public partial class User
    {
        public User()
        {
            Comments = new List<Comment>();
            LikesDislikes = new List<LikesDislike>();
            Recipes = new List<Recipe>();
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

        [JsonIgnore]
        [InverseProperty(nameof(LikesDislike.User))]
        public virtual IEnumerable<LikesDislike> LikesDislikes { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(Recipe.User))]
        public virtual IEnumerable<Recipe> Recipes { get; set; }
    }
}

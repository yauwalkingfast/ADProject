using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            LikesDislikes = new HashSet<LikesDislike>();
            Recipes = new HashSet<Recipe>();
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

        [InverseProperty(nameof(Comment.User))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(LikesDislike.User))]
        public virtual ICollection<LikesDislike> LikesDislikes { get; set; }
        [InverseProperty(nameof(Recipe.User))]
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}

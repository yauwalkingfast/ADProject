using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Table("Recipe")]
    public partial class Recipe
    {
        public Recipe()
        {
            Comments = new HashSet<Comment>();
            LikesDislikes = new HashSet<LikesDislike>();
            RecipeIngredients = new HashSet<RecipeIngredient>();
            RecipeSteps = new HashSet<RecipeStep>();
        }

        [Key]
        public int RecipeId { get; set; }
        [Required]
        [Column("title")]
        [StringLength(100)]
        public string Title { get; set; }
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; }
        public int UserId { get; set; }
        [Column("dateCreated", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("durationInMins")]
        public int DurationInMins { get; set; }
        [Column("calories")]
        public int? Calories { get; set; }
        [Column("servingSize")]
        public int? ServingSize { get; set; }
        [Column("isPublished")]
        public bool? IsPublished { get; set; }
        [Column("mainMediaUrl")]
        [StringLength(200)]
        public string MainMediaUrl { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Recipes")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(Comment.Recipe))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(LikesDislike.Recipe))]
        public virtual ICollection<LikesDislike> LikesDislikes { get; set; }
        [InverseProperty(nameof(RecipeIngredient.Recipe))]
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        [InverseProperty(nameof(RecipeStep.Recipe))]
        public virtual ICollection<RecipeStep> RecipeSteps { get; set; }
    }
}

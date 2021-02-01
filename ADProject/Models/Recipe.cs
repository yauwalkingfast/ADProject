using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ADProject.Models
{
    [Table("Recipe")]
    public partial class Recipe
    {
        public Recipe()
        {
            Comments = new List<Comment>();
            LikesDislikes = new List<LikesDislike>();
            RecipeIngredients = new List<RecipeIngredient>();
            RecipeSteps = new List<RecipeStep>();
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

        /*[InverseProperty(nameof(LikesDislike.Recipe))]
        public virtual ICollection<LikesDislike> LikesDislikes { get; set; }
        [InverseProperty(nameof(RecipeIngredient.Recipe))]
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        [InverseProperty(nameof(RecipeStep.Recipe))]
        public virtual ICollection<RecipeStep> RecipeSteps { get; set; }*/

        [InverseProperty(nameof(Comment.Recipe))]

        public virtual IEnumerable<Comment> Comments { get; set; }

        [InverseProperty(nameof(LikesDislike.Recipe))]
        public virtual IEnumerable<LikesDislike> LikesDislikes { get; set; }
        [InverseProperty(nameof(RecipeIngredient.Recipe))]
        public virtual IEnumerable<RecipeIngredient> RecipeIngredients { get; set; }
        [InverseProperty(nameof(RecipeStep.Recipe))]
        public virtual IEnumerable<RecipeStep> RecipeSteps { get; set; }

        public static implicit operator List<object>(Recipe v)
        {
            throw new NotImplementedException();
        }
    }
}

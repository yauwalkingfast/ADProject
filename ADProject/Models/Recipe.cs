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
            Comments = new List<Comment>();
            LikesDislikes = new List<LikesDislike>();
            RecipeIngredients = new List<RecipeIngredient>();
            RecipeSteps = new List<RecipeStep>();
            RecipeTags = new List<RecipeTag>();
            RecipeGroups = new List<RecipeGroup>();
            SavedRecipes = new List<SavedRecipe>();
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

        // [DataType(DataType.Upload)]
        // [Display(Name = "Upload File")]
        // [Required(ErrorMessage = "Please choose file to upload.")]
        public string MainMediaUrl { get; set; }

        [ForeignKey(nameof(UserId))]
        [InverseProperty("Recipes")]
        public virtual ApplicationUser User { get; set; }
        [InverseProperty(nameof(Comment.Recipe))]
        public virtual IEnumerable<Comment> Comments { get; set; }
        [InverseProperty(nameof(LikesDislike.Recipe))]
        public virtual IEnumerable<LikesDislike> LikesDislikes { get; set; }
        [InverseProperty(nameof(RecipeGroup.Recipe))]
        public virtual List<RecipeGroup> RecipeGroups { get; set; }
        [InverseProperty(nameof(RecipeIngredient.Recipe))]
        public virtual List<RecipeIngredient> RecipeIngredients { get; set; }
        [InverseProperty(nameof(RecipeStep.Recipe))]
        public virtual List<RecipeStep> RecipeSteps { get; set; }
        [InverseProperty(nameof(RecipeTag.Recipe))]
        public virtual IEnumerable<RecipeTag> RecipeTags { get; set; }
       
        public static implicit operator List<object>(Recipe v)
        {
            throw new NotImplementedException();
        }


        [InverseProperty(nameof(SavedRecipe.Recipe))]
        public virtual List<SavedRecipe> SavedRecipes { get; set; }
    }
}

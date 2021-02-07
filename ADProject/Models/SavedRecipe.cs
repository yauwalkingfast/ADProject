using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class SavedRecipe
    {
        [Key]
        public int SavedRecipesId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("SavedRecipes")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("SavedRecipes")]
        public virtual User User { get; set; }
    }
}

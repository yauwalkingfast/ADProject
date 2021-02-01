using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class RecipeIngredient
    {
        [Key]
        public int RecipeIngredientsId { get; set; }
        public int RecipeId { get; set; }
        [Required]
        [Column("ingredient")]
        [StringLength(100)]
        public string Ingredient { get; set; }
        [Column("quantity")]
        public double Quantity { get; set; }
        [Column("unitOfMeasurement")]
        [StringLength(20)]
        public string UnitOfMeasurement { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("RecipeIngredients")]
        public virtual Recipe Recipe { get; set; }
    }
}

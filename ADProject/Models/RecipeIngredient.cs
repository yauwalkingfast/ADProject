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
    public partial class RecipeIngredient
    {
        [Key]
        public int RecipeIngredientsId { get; set; }
        public int RecipeId { get; set; }
        [Required]
        [Column("ingredient")]
        [StringLength(20)]
        public string Ingredient { get; set; }
        [Column("quantity")]
        public double Quantity { get; set; }
        [Column("unitOfMeasurement")]
        [StringLength(20)]
        public string UnitOfMeasurement { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("RecipeIngredients")]
        public virtual Recipe Recipe { get; set; }
    }
}

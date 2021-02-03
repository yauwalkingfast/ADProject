using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class RecipeTag
    {
        [Key]
        [Column("RecipeTagID")]
        public int RecipeTagId { get; set; }
        public int RecipeId { get; set; }
        public int TagId { get; set; }
        [Column("isAllergenTag")]
        public bool IsAllergenTag { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("RecipeTags")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("RecipeTags")]
        public virtual Tag Tag { get; set; }
    }
}

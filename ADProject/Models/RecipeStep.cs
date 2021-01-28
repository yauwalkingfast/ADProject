using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class RecipeStep
    {
        [Key]
        public int RecipeStepsId { get; set; }
        public int RecipeId { get; set; }
        [Column("stepNumber")]
        public int StepNumber { get; set; }
        [Column("textInstructions")]
        [StringLength(400)]
        public string TextInstructions { get; set; }
        [Column("mediaFileURL")]
        [StringLength(200)]
        public string MediaFileUrl { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("RecipeSteps")]
        public virtual Recipe Recipe { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Keyless]
    public partial class SavedRecipe
    {
        public int UserId { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}

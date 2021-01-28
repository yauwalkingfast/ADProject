using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Keyless]
    public partial class RecipeTag
    {
        public int RecipeId { get; set; }
        public int TagId { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; }
    }
}

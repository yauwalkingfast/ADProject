using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Keyless]
    [Table("RecipeGroup")]
    public partial class RecipeGroup
    {
        public int GroupId { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public virtual Recipe Recipe { get; set; }
    }
}

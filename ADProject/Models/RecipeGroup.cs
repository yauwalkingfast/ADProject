using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Table("RecipeGroup")]
    public partial class RecipeGroup
    {
        [Key]
        public int RecipeGroupId { get; set; }
        public int GroupId { get; set; }
        public int RecipeId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("RecipeGroups")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("RecipeGroups")]
        public virtual Recipe Recipe { get; set; }
        
    }
}

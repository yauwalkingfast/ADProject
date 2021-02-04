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
    [Table("Tag")]
    public partial class Tag
    {
        public Tag()
        {
            RecipeTags = new HashSet<RecipeTag>();
        }

        [Key]
        public int TagId { get; set; }
        [Required]
        [Column("tagName")]
        [StringLength(30)]
        public string TagName { get; set; }
        [Required]
        [Column("warning")]
        [StringLength(200)]
        public string Warning { get; set; }

        [JsonIgnore]
        [InverseProperty(nameof(RecipeTag.Tag))]
        public virtual ICollection<RecipeTag> RecipeTags { get; set; }
    }
}

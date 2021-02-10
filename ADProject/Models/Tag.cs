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
            GroupTags = new List<GroupTag>();
            RecipeTags = new List<RecipeTag>();
            UserAllergens = new List<UserAllergen>();
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
        [InverseProperty(nameof(GroupTag.Tag))]
        public virtual IEnumerable<GroupTag> GroupTags { get; set; }
        [InverseProperty(nameof(RecipeTag.Tag))]

        [JsonIgnore]
        public virtual IEnumerable<RecipeTag> RecipeTags { get; set; }
        [InverseProperty(nameof(UserAllergen.Tag))]
        public virtual IEnumerable<UserAllergen> UserAllergens { get; set; }
    }
}

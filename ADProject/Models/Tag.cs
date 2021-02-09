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
            GroupTags = new HashSet<GroupTag>();
            RecipeTags = new HashSet<RecipeTag>();
            UserAllergens = new HashSet<UserAllergen>();
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
        public virtual ICollection<GroupTag> GroupTags { get; set; }
        [InverseProperty(nameof(RecipeTag.Tag))]
        [JsonIgnore]
        public virtual ICollection<RecipeTag> RecipeTags { get; set; }
        [InverseProperty(nameof(UserAllergen.Tag))]
        public virtual ICollection<UserAllergen> UserAllergens { get; set; }
    }
}

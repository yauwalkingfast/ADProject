using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace ADProject.Models
{
    [Table("Group")]
    public partial class Group
    {
        public Group()
        {
            GroupTags = new HashSet<GroupTag>();
            RecipeGroups = new HashSet<RecipeGroup>();
            UsersGroups = new HashSet<UsersGroup>();
        }

        [Key]
        public int GroupId { get; set; }
        [Required]
        [Column("groupName")]
        [StringLength(50)]
        public string GroupName { get; set; }
        [Column("groupPhoto")]
        [StringLength(500)]
        public string GroupPhoto { get; set; }
        [Column("description")]
        [StringLength(500)]
        public string Description { get; set; }
        [Column("dateCreated", TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column("isPublished")]
        public bool IsPublished { get; set; }

        [InverseProperty(nameof(GroupTag.Group))]
        public virtual ICollection<GroupTag> GroupTags { get; set; }
        [InverseProperty(nameof(RecipeGroup.Group))]
        public virtual ICollection<RecipeGroup> RecipeGroups { get; set; }
        [InverseProperty(nameof(UsersGroup.Group))]
        public virtual ICollection<UsersGroup> UsersGroups { get; set; }

        [NotMapped]
        public IFormFile GroupPicture { get; set; }
    }
}

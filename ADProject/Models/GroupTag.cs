using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class GroupTag
    {
        [Key]
        public int GroupTagsId { get; set; }
        public int TagId { get; set; }
        public int GroupId { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("GroupTags")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(TagId))]
        [InverseProperty("GroupTags")]
        public virtual Tag Tag { get; set; }
    }
}

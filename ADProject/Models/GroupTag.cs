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
    public partial class GroupTag
    {
        [Key]
        public int GroupTagsId { get; set; }
        public int TagId { get; set; }
        public int GroupId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(GroupId))]
        [InverseProperty("GroupTags")]
        public virtual Group Group { get; set; }

        [ForeignKey(nameof(TagId))]
        [InverseProperty("GroupTags")]
        public virtual Tag Tag { get; set; }
    }
}

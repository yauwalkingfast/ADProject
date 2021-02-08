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
    [Table("UsersGroup")]
    public partial class UsersGroup
    {
        [Key]
        public int UsersGroupId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public bool IsMod { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("UsersGroups")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UsersGroups")]
        public virtual User User { get; set; }
    }
}

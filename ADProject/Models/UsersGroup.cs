using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Keyless]
    [Table("UsersGroup")]
    public partial class UsersGroup
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public bool IsMod { get; set; }

        [ForeignKey(nameof(GroupId))]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Keyless]
    public partial class FollowUser
    {
        [Column("followingUserId")]
        public int FollowingUserId { get; set; }
        [Column("followedUserId")]
        public int FollowedUserId { get; set; }

        [ForeignKey(nameof(FollowedUserId))]
        public virtual User FollowedUser { get; set; }
        [ForeignKey(nameof(FollowingUserId))]
        public virtual User FollowingUser { get; set; }
    }
}

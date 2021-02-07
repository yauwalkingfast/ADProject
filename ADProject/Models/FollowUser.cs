using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class FollowUser
    {
        [Key]
        public int FollowUsersId { get; set; }
        [Column("followingUserId")]
        public int FollowingUserId { get; set; }
        [Column("followedUserId")]
        public int FollowedUserId { get; set; }

        [ForeignKey(nameof(FollowedUserId))]
        [InverseProperty(nameof(User.FollowUserFollowedUsers))]
        public virtual User FollowedUser { get; set; }
        [ForeignKey(nameof(FollowingUserId))]
        [InverseProperty(nameof(User.FollowUserFollowingUsers))]
        public virtual User FollowingUser { get; set; }
    }
}

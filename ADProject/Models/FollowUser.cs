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
        [InverseProperty(nameof(ApplicationUser.FollowUserFollowedUsers))]
        public virtual ApplicationUser FollowedUser { get; set; }
        [ForeignKey(nameof(FollowingUserId))]
        [InverseProperty(nameof(ApplicationUser.FollowUserFollowingUsers))]
        public virtual ApplicationUser FollowingUser { get; set; }
        
    }
}

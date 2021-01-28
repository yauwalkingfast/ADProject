using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class LikesDislike
    {
        [Key]
        public int LikesDislikesId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        [Column("isLiked")]
        public bool IsLiked { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("LikesDislikes")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("LikesDislikes")]
        public virtual User User { get; set; }
    }
}

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
    public partial class LikesDislike
    {
        [Key]
        public int LikesDislikesId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        [Column("isLiked")]
        public bool IsLiked { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("LikesDislikes")]
        public virtual Recipe Recipe { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        [InverseProperty("LikesDislikes")]
        public virtual ApplicationUser User { get; set; }
        
    }
}

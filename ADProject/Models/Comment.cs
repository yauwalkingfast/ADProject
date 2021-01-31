using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    public partial class Comment
    {
        [Key]
        [Column("commentsId")]
        public int CommentsId { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        [Column("dateposted", TypeName = "datetime")]
        public DateTime Dateposted { get; set; }
        [Required]
        [Column("comment")]
        [StringLength(500)]
        public string Comment1 { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("Comments")]
        public virtual Recipe Recipe { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Comments")]
        public virtual User User { get; set; }
    }
}

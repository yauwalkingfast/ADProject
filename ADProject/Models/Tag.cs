using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Table("Tag")]
    public partial class Tag
    {
        [Key]
        public int TagId { get; set; }
        [Required]
        [Column("tagName")]
        [StringLength(30)]
        public string TagName { get; set; }
        [Required]
        [Column("warning")]
        [StringLength(200)]
        public string Warning { get; set; }
    }
}

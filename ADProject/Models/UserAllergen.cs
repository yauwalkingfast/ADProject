using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace ADProject.Models
{
    [Table("UserAllergen")]
    public partial class UserAllergen
    {
        [Key]
        public int UserAllergenId { get; set; }
        public int UserId { get; set; }
        public int TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        [InverseProperty("UserAllergens")]
        public virtual Tag Tag { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserAllergens")]
        public virtual ApplicationUser User { get; set; }
    }
}

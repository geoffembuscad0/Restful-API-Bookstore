using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestfulApiDemo.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Column("deleted_at")]
        public DateTime? DeletedAt { get; set; }
    }
}
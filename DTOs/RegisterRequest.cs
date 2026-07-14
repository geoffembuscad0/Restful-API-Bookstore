using System.ComponentModel.DataAnnotations;

namespace RestfulApiDemo.DTOs
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(3)]
        [MaxLength(255)]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
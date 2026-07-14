using System.ComponentModel.DataAnnotations;

namespace RestfulApiDemo.DTOs
{
    public class CategoryRequest
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
    }
}
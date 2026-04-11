using System.ComponentModel.DataAnnotations;

namespace MyStore.Models
{
    public class UserVM
    {
        public int UsertId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Password2 { get; set; }
        [Required]
        public string Type { get; set; }

    }
}

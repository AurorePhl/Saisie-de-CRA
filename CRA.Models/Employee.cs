using System.ComponentModel.DataAnnotations;

namespace CRA.Models
{
    public class Employee
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name must contain only letters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Surname must contain only letters.")]
        public string Surname { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Username must contain only letters.")]
        public string Username { get; set; }

        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must contain upper, lower, digit, and special character.")]
        public string Password { get; set; }

        [Required]
        public bool IsConnected { get; set; }

        [Required]
        public bool Role { get; set; }
    }
}

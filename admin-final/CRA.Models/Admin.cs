using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Models
{
    public class Admin
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

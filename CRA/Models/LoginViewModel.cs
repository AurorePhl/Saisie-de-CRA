using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRA.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]  // Masque le mot de passe
        public string Password { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CRA.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Nom d'utilisateur requis.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mot de passe requis.")]
        [DataType(DataType.Password)]  // Masque le mot de passe
        public string Password { get; set; }
    }
}

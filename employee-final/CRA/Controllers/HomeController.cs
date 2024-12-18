using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;

namespace CRA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminRepository _adminRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository, IAdminRepository adminRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _adminRepository = adminRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);  // Retourner la vue si le modèle est invalide
            }
            // Vérifier si le nom d'utilisateur ou le mot de passe est null
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                return NotFound(); // Retourne 404 si l'un des deux paramètres est vide
            }

            // Récupérer la liste des employés et des administrateurs
            IEnumerable<Employee> employees = _employeeRepository.GetAllEmployees();
            IEnumerable<Admin> admins = _adminRepository.GetAllAdmin();

            // Vérification dans la liste des employés
            var employee = employees.FirstOrDefault(e => e.Username == model.Username && e.Password == model.Password);
            if (employee != null)
            {
                // Si l'employé existe, rediriger vers HomeEmployee
                return RedirectToRoute(new
                {
                    controller = "HomeEmployee",
                    action = "Index",
                    id = employee.Id
                });
            }

            // Vérification dans la liste des administrateurs
            var admin = admins.FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);
            if (admin != null)
            {
                return RedirectToRoute(new
                {
                    controller = "HomeAdmin",
                    action = "Index",
                    id = admin.Id
                });
            }

            ModelState.AddModelError("", "Invalid Username or Password."); 
            return View(model);
        }

    }
}

using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace CRA.Controllers
{
    public class HomeAdminController : Controller
    {
        //private readonly IAssignmentRepository _repository; // injection de dépendance
        private readonly IAdminRepository _repositoryAdmin;
        public HomeAdminController(IAdminRepository repositoryAdmin) // constructeur, repository = variable local de IAssignmentRepository
        {
            //_repository = repository; // initialisation de la variable _repository
            _repositoryAdmin = repositoryAdmin;
        }
        public IActionResult Index(Guid id)
        {
            var admin = _repositoryAdmin.GetAdminById(id);
            if (admin == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = id;
            return View(admin);
        }

        public IActionResult Assignments(Guid id)
        {
            
            ViewData["AdminId"] = id;
            // Redirige vers l'action Index du contrôleur AssignmentController
            return RedirectToAction("Index", "Assignment", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout(Guid id)
        {
            ViewData["AdminId"] = id;
            return RedirectToRoute(new
            {
                controller = "HomeAdmin",
                action = "Index",
                id = id
            });
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
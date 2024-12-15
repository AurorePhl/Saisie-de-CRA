using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace CRA.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Assignments(int adminId)
        {
            // Redirige vers l'action Index du contrôleur AssignmentController
            return RedirectToAction("Index", "Assignment", new { id = adminId });
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
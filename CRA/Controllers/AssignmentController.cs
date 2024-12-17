using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
namespace app.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentRepository _repository; // injection de dépendance
        private readonly IAdminRepository _repositoryAdmin;
        private readonly IEmployeeRepository _repositoryEmployee;
        private readonly IPeriodRepository _repositoryPeriod;
        public AssignmentController(IAdminRepository repositoryAdmin, IAssignmentRepository repository, IEmployeeRepository repositoryEmployee, IPeriodRepository repositoryPeriod) // constructeur, repository = variable local de IAssignmentRepository
        {
            _repository = repository; // initialisation de la variable _repository
            _repositoryAdmin = repositoryAdmin;
            _repositoryEmployee = repositoryEmployee;
            _repositoryPeriod = repositoryPeriod;
        }
        public IActionResult Index(Guid adminId)
        {
            // Récupérer les assignments avec leurs détails (Username, Dates, etc.)
            var assignments = _repository.GetAllAssignmentsWithDetails();

            // Retourner les données à la vue

            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Assignment/Index.cshtml", assignments);
        }


        //public IActionResult Create(Guid adminId)
        //{
        //    // a definir
        //    ViewData["AdminId"] = adminId;
        //    return View("/Views/Admin/Assignment/Create.cshtml");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken] // vérification de la validité du jeton anti-forgery

        //public IActionResult Create(Assignment assignment, Guid adminId)
        //{

        //    ViewData["AdminId"] = adminId;
        //    return View("/Views/Admin/Assignment/Create.cshtml", assignment);
        //}

        //public IActionResult Edit(Guid code, Guid adminId) // afficher le formulaire HTML pour modifie un enregistrement dans la base de données
        //{

        //    ViewData["AdminId"] = adminId;
        //    return View("/Views/Admin/Assignment/Edit.cshtml");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(Assignment assignment, Guid adminId) // appelée lorsque le formulaire HTML de modification d'une course est posté sur le serveur
        //{
        //    ViewData["AdminId"] = adminId;
        //    return View("/Views/Admin/Assignment/Edit.cshtml", assignment);
        //}

        //public IActionResult Details(Guid code, Guid adminId)
        //{

        //    ViewData["AdminId"] = adminId;
        //    return View("/Views/Admin/Assignment/Details.cshtml");
        //}

        //public IActionResult Delete(Guid code, Guid adminId)
        //{

        //    ViewData["AdminId"] = adminId;
        //    return View("/Views/Admin/Assignment/Delete.cshtml"); // retourne la vue de l'assignment
        //}
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(Guid code, Guid adminId)
        //{
        //    _repository.DeleteAssignment(code); // supprime l'assignment
        //    return RedirectToAction(nameof(Index)); // redirige vers l'index
        //}
    }
}

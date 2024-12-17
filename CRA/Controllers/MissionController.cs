using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
namespace app.Controllers
{
    public class MissionController : Controller
    {
        private readonly IAssignmentRepository _repository; // injection de dépendance
        private readonly IAdminRepository _repositoryAdmin;
        public MissionController(IAdminRepository repositoryAdmin, IAssignmentRepository repository) // constructeur, repository = variable local de IAssignmentRepository
        {
            _repository = repository; // initialisation de la variable _repository
            _repositoryAdmin = repositoryAdmin;
        }
        public IActionResult Index(Guid id)
        {
            
            IEnumerable<Assignment> assignments = _repository.GetAllAssignments(); // récupérer tous les assignments
            ViewData["AdminId"] = id;
            return View("/Views/Admin/Mission/Index.cshtml", assignments);
        }

        public IActionResult Create(Guid adminId)
        {
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // vérification de la validité du jeton anti-forgery

        public IActionResult Create(Assignment assignment, Guid adminId)
        {
            if (ModelState.IsValid) // vérifier si le modèle est valide
            {

                assignment.Code = Guid.NewGuid();
                _repository.AddAssignment(assignment); // ajouter un assignment
                return RedirectToRoute(new
                {
                    controller = "Mission",
                    action = "Index",
                    id = adminId
                }); // rediriger vers l'index
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Create.cshtml", assignment);
        }

        public IActionResult Edit(Guid code, Guid adminId) // afficher le formulaire HTML pour modifie un enregistrement dans la base de données
        {
            var assignment = _repository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Edit.cshtml", assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Assignment assignment, Guid adminId) // appelée lorsque le formulaire HTML de modification d'une course est posté sur le serveur
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateAssignment(assignment);
                return RedirectToRoute(new
                {
                    controller = "Mission",
                    action = "Index",
                    id = adminId
                });
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Edit.cshtml", assignment);
        }

        public IActionResult Details(Guid code, Guid adminId)
        {
            var assignment = _repository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Details.cshtml", assignment);
        }

        public IActionResult Delete(Guid code, Guid adminId)
        {
            var assignment = _repository.GetAssignmentByCode(code); // récupère l'assignment par son code
            if (assignment == null)
            // si l'assignment n'existe pas
            {
                return NotFound(); // retourne une erreur 404
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Delete.cshtml", assignment); // retourne la vue de l'assignment
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid code, Guid adminId)
        {
            _repository.DeleteAssignment(code); // supprime l'assignment
            return RedirectToRoute(new
            {
                controller = "Mission",
                action = "Index",
                id = adminId
            }); // redirige vers l'index
            //return RedirectToAction(nameof(Index)); // redirige vers l'index
        }
    }
}

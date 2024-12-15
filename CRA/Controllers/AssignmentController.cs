using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
namespace app.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentRepository _repository; // injection de dépendance
        public AssignmentController(IAssignmentRepository repository) // constructeur, repository = variable local de IAssignmentRepository
        {
            _repository = repository; // initialisation de la variable _repository
        }
        public IActionResult Index()
        {
            IEnumerable<Assignment> assignments = _repository.GetAllAssignments(); // récupérer tous les assignments
            return View("/Views/Admin/Assignment/Index.cshtml", assignments);
        }
        public IActionResult Create()
        {
            return View("/Views/Admin/Assignment/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // vérification de la validité du jeton anti-forgery

        public IActionResult Create(Assignment assignment)
        {
            if (ModelState.IsValid) // vérifier si le modèle est valide
            {
                _repository.AddAssignment(assignment); // ajouter un assignment
                return RedirectToAction(nameof(Index)); // rediriger vers l'index
            }
            return View("/Views/Admin/Assignment/Create.cshtml", assignment);
        }

        public IActionResult Edit(Guid code) // afficher le formulaire HTML pour modifie un enregistrement dans la base de données
        {
            var assignment = _repository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound();
            }
            return View("/Views/Admin/Assignment/Edit.cshtml", assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Assignment assignment) // appelée lorsque le formulaire HTML de modification d'une course est posté sur le serveur
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateAssignment(assignment);
                return RedirectToAction(nameof(Index));
            }
            return View("/Views/Admin/Assignment/Edit.cshtml", assignment);
        }

        public IActionResult Details(Guid code)
        {
            var assignment = _repository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound();
            }
            return View("/Views/Admin/Assignment/Details.cshtml", assignment);
        }
        public IActionResult Delete(Guid code)
        {
            var assignment = _repository.GetAssignmentByCode(code); // récupère l'assignment par son code
            if (assignment == null)
            // si l'assignment n'existe pas
            {
                return NotFound(); // retourne une erreur 404
            }
            return View("/Views/Admin/Assignment/Delete.cshtml", assignment); // retourne la vue de l'assignment
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid code)
        {
            _repository.DeleteAssignment(code); // supprime l'assignment
            return RedirectToAction(nameof(Index)); // redirige vers l'index
        }
    }
}

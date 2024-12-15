using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRA.Controllers
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
            return View(assignments);
        }
    }
}

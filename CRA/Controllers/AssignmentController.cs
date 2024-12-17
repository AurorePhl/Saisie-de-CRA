using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace app.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentRepository _repository; // injection de dépendance
        private readonly IAdminRepository _repositoryAdmin;
        private readonly IEmployeeRepository _repositoryEmployee;
        private readonly IPeriodRepository _repositoryPeriod;
        private readonly ApplicationDbContext _context;

        public AssignmentController(IAdminRepository repositoryAdmin, IAssignmentRepository repository, IEmployeeRepository repositoryEmployee, IPeriodRepository repositoryPeriod, ApplicationDbContext context) // constructeur, repository = variable local de IAssignmentRepository
        {
            _repository = repository; // initialisation de la variable _repository
            _repositoryAdmin = repositoryAdmin;
            _repositoryEmployee = repositoryEmployee;
            _repositoryPeriod = repositoryPeriod;
            _context = context;
        }
        public IActionResult Index(Guid id, string searchString)
        {
            // Récupérer les assignments avec leurs détails (Username, Dates, etc.)
            var assignments = _repository.GetAllAssignmentsWithDetails();
            // Retourner les données à la vue
            if (!string.IsNullOrEmpty(searchString))
            {
                assignments = assignments.Where(t => t.Libelle.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            ViewData["AdminId"] = id;
            return View("/Views/Admin/Assignment/Index.cshtml", assignments);
        }

        public IActionResult Create(Guid adminId)
        {
            // Récupérer tous les employés pour la liste déroulante
            var employees = _context.Employee
                                   .Select(e => new { e.Username })
                                   .ToList();

            // Passer la liste des employés à la vue
            ViewBag.EmployeeUsernames = new SelectList(employees, "Username", "Username");

            // Passer AdminId pour maintenir le contexte
            ViewData["AdminId"] = adminId;

            return View("/Views/Admin/Assignment/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string Libelle, string Description, string Username, DateTime Start, DateTime End, Guid adminId)
        {
            try
            {
                // récupérer le ScheduleId de l'employé via son Username
                var employee = _context.Employee.FirstOrDefault(e => e.Username == Username);
                if (employee == null)
                {
                    ModelState.AddModelError("", "L'employé avec le nom d'utilisateur spécifié n'existe pas.");
                    return View("/Views/Admin/Assignment/Create.cshtml");
                }

                // trouver le bon schedule avec l'username de l'employé
                var schedule = _context.Schedule.FirstOrDefault(s => s.EmployeeId == employee.Id);
                if (schedule == null)
                {
                    ModelState.AddModelError("", "Aucun schedule n'est associé à cet employé.");
                    return View("/Views/Admin/Assignment/Create.cshtml");
                }

                // créer une nouvelle Period
                var newPeriod = new Period
                {
                    Start = Start,
                    End = End
                };
                _context.Period.Add(newPeriod);
                _context.SaveChanges(); // Sauvegarder pour générer l'ID de la période

                // créer un nouvel Assignment
                var newAssignment = new Assignment
                {
                    Libelle = Libelle,
                    Description = Description,
                    ScheduleId = schedule.Id, // ScheduleId récupéré
                    AdminId = adminId, // AdminId passé depuis la vue
                    PeriodId = newPeriod.Id, // ID de la nouvelle période
                    IsAssigned = true, 
                    IsValidated = false
                };

                _context.Assignment.Add(newAssignment);
                _context.SaveChanges();

                // redirection vers la page Index 
                return RedirectToAction("Index", new { id = adminId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Une erreur s'est produite lors de la création de l'assignation : " + ex.Message);
                return View("/Views/Admin/Assignment/Create.cshtml");
            }
        }

        //public IActionResult Edit(Guid code, Guid adminId) // afficher le formulaire HTML pour modifie un enregistrement dans la base de données
        //{
        //    var assignment = _repository.GetAssignmentByCode(code);
        //    if (assignment == null)
        //    {
        //        return NotFound();
        //    }
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

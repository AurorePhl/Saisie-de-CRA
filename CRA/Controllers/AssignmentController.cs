using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CRA.Controllers
{
    public class AssignmentController : Controller
    {
        private readonly IAssignmentRepository _repository;
        private readonly IAdminRepository _repositoryAdmin;
        private readonly IEmployeeRepository _repositoryEmployee;
        private readonly IPeriodRepository _repositoryPeriod;
        private readonly ApplicationDbContext _context;

        public AssignmentController(
            IAssignmentRepository repository,
            IAdminRepository repositoryAdmin,
            IEmployeeRepository repositoryEmployee,
            IPeriodRepository repositoryPeriod,
            ApplicationDbContext context)
        {
            _repository = repository;
            _repositoryAdmin = repositoryAdmin;
            _repositoryEmployee = repositoryEmployee;
            _repositoryPeriod = repositoryPeriod;
            _context = context;
        }

        // Index: Liste les assignments
        public IActionResult Index(Guid id, string searchString)
        {
            var assignments = _repository.GetAllAssignmentsWithDetails();
            if (!string.IsNullOrEmpty(searchString))
            {
                assignments = assignments.Where(t => t.Libelle.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }
            ViewData["AdminId"] = id;
            return View("/Views/Admin/Assignment/Index.cshtml", assignments);
        }

        // Create: Affiche le formulaire de création
        public IActionResult Create(Guid adminId)
        {
            var employees = _context.Employee.Select(e => new { e.Username }).ToList();
            ViewBag.EmployeeUsernames = new SelectList(employees, "Username", "Username");
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Assignment/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string Libelle, string Description, string Username, DateTime Start, DateTime End, Guid adminId)
        {
            try
            {
                var employee = _context.Employee.FirstOrDefault(e => e.Username == Username);
                if (employee == null)
                {
                    ModelState.AddModelError("", "L'employé avec le nom d'utilisateur spécifié n'existe pas.");
                    return View("/Views/Admin/Assignment/Create.cshtml");
                }

                var schedule = _context.Schedule.FirstOrDefault(s => s.EmployeeId == employee.Id);
                if (schedule == null)
                {
                    ModelState.AddModelError("", "Aucun schedule n'est associé à cet employé.");
                    return View("/Views/Admin/Assignment/Create.cshtml");
                }

                var newPeriod = new Period { Start = Start, End = End };
                _context.Period.Add(newPeriod);
                _context.SaveChanges();

                var newAssignment = new Assignment
                {
                    Libelle = Libelle,
                    Description = Description,
                    ScheduleId = schedule.Id,
                    AdminId = adminId,
                    PeriodId = newPeriod.Id,
                    IsAssigned = true,
                    IsValidated = false
                };

                _context.Assignment.Add(newAssignment);
                _context.SaveChanges();
                ViewData["AdminId"] = adminId;
                return RedirectToAction("Index", new { id = adminId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Une erreur s'est produite : " + ex.Message);
                ViewData["AdminId"] = adminId;
                return View("/Views/Admin/Assignment/Create.cshtml");
            }
        }

        // Edit: Affiche le formulaire d'édition
        public IActionResult Edit(Guid code, Guid adminId)
        {
            var assignment = _repository.GetAssignmentByCode(code);
            if (assignment == null) return NotFound();

            var employees = _context.Employee.Select(e => new { e.Username }).ToList();
            ViewBag.EmployeeUsernames = new SelectList(employees, "Username", "Username");
            ViewData["AdminId"] = adminId;

            var schedule = _context.Schedule.FirstOrDefault(s => s.Id == assignment.ScheduleId);
            var employee = schedule != null ? _context.Employee.FirstOrDefault(e => e.Id == schedule.EmployeeId) : null;
            var period = _context.Period.FirstOrDefault(p => p.Id == assignment.PeriodId);

            var assignmentViewModel = new AssignmentViewModel
            {
                Code = assignment.Code,
                Libelle = assignment.Libelle,
                Description = assignment.Description,
                Username = employee?.Username,
                Start = period?.Start,
                End = period?.End
            };

            return View("/Views/Admin/Assignment/Edit.cshtml", assignmentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AssignmentViewModel model, Guid adminId)
        {
            if (!ModelState.IsValid)
            {
                var employees = _context.Employee.Select(e => new { e.Username }).ToList();
                ViewBag.EmployeeUsernames = new SelectList(employees, "Username", "Username");
                ViewData["AdminId"] = adminId;
                return View("/Views/Admin/Assignment/Edit.cshtml", model);
            }

            try
            {
                var assignment = _repository.GetAssignmentByCode(model.Code);
                if (assignment == null) return NotFound();

                var employee = _context.Employee.FirstOrDefault(e => e.Username == model.Username);
                if (employee == null)
                {
                    ModelState.AddModelError("", "L'employé avec le nom d'utilisateur spécifié n'existe pas.");
                    ViewData["AdminId"] = adminId;
                    return View("/Views/Admin/Assignment/Edit.cshtml", model);
                }

                var schedule = _context.Schedule.FirstOrDefault(s => s.EmployeeId == employee.Id);
                if (schedule == null)
                {
                    ModelState.AddModelError("", "Aucun schedule n'est associé à cet employé.");
                    ViewData["AdminId"] = adminId;
                    return View("/Views/Admin/Assignment/Edit.cshtml", model);
                }

                var period = _context.Period.FirstOrDefault(p => p.Id == assignment.PeriodId);
                if (period == null)
                {
                    period = new Period { Start = model.Start.Value, End = model.End.Value };
                    _context.Period.Add(period);
                    _context.SaveChanges();
                    assignment.PeriodId = period.Id;
                }
                else
                {
                    period.Start = model.Start.Value;
                    period.End = model.End.Value;
                    _context.Period.Update(period);
                }

                assignment.Libelle = model.Libelle;
                assignment.Description = model.Description;
                assignment.ScheduleId = schedule.Id;

                _repository.UpdateAssignment(assignment);
                _context.SaveChanges();
                ViewData["AdminId"] = adminId;
                return RedirectToAction("Index", new { id = adminId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Une erreur s'est produite : " + ex.Message);
                ViewData["AdminId"] = adminId;
                return View("/Views/Admin/Assignment/Edit.cshtml", model);
            }
        }
    }
}

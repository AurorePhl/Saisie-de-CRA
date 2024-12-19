using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRA.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IAssignmentRepository _repository;
        private readonly IAdminRepository _repositoryAdmin;
        private readonly IEmployeeRepository _repositoryEmployee;
        private readonly IPeriodRepository _repositoryPeriod;
        private readonly ITimeSlotRepository _repositoryTimeSlot;
        private readonly ApplicationDbContext _context;

        public ScheduleController(
            IAdminRepository repositoryAdmin,
            IAssignmentRepository repository,
            IEmployeeRepository repositoryEmployee,
            IPeriodRepository repositoryPeriod,
            ITimeSlotRepository repositoryTimeSlot,
            ApplicationDbContext context)
        {
            _repository = repository;
            _repositoryAdmin = repositoryAdmin;
            _repositoryEmployee = repositoryEmployee;
            _repositoryPeriod = repositoryPeriod;
            _context = context;
            _repositoryTimeSlot = repositoryTimeSlot;
        }

        public IActionResult Index(Guid id, string searchString)
        {

            // Récupérer les assignments avec leurs détails (Username, Dates, etc.)
            var timeslots = _repositoryTimeSlot.SentTimeSlots();

            // Filtrer les résultats en fonction du Username
            if (!string.IsNullOrEmpty(searchString))
            {
                timeslots = timeslots.Where(t => t.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase));
            }

            // Trier les résultats pour afficher les états "sent" en premier
            var sortedTimeslots = timeslots
                .OrderByDescending(t => t.State == "SENT")
                .ThenBy(t => t.State)
                .ToList();

            // Retourner les données à la vue
            ViewData["AdminId"] = id;
            ViewData["SearchString"] = searchString;
            return View("/Views/Admin/Schedule/Index.cshtml", sortedTimeslots);
        }
        public IActionResult Edit(Guid idTimeSlot, Guid adminId) // afficher le formulaire HTML pour modifie un enregistrement dans la base de données
        {
            var timeslot = _repositoryTimeSlot.GetTimeSlotById(idTimeSlot);
            if (timeslot == null)
            {
                return NotFound();
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Schedule/Edit.cshtml", timeslot);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TimeSlot timeslot, Guid adminId) // appelée lorsque le formulaire HTML de modification d'une course est posté sur le serveur
        {
            if (timeslot == null)
            {
                return NotFound("Créneau horaire introuvable.");
            }

            if (ModelState.IsValid)
            {
                _repositoryTimeSlot.UpdateTimeSlot(timeslot);
                return RedirectToRoute(new
                {
                    controller = "Schedule",
                    action = "Index",
                    id = adminId
                });
            }
            ViewData["AdminId"] = adminId;
            return View("/Views/Admin/Mission/Edit.cshtml", timeslot);
        }
    }
}
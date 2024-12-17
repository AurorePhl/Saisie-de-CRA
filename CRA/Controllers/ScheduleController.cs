using CRA.DataAccess;
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

        public IActionResult Index(Guid id)
        {
            // Récupérer les assignments avec leurs détails (Username, Dates, etc.)
            var timeslot = _repositoryTimeSlot.SentTimeSlots();
            // Retourner les données à la vue
            ViewData["AdminId"] = id;
            return View("/Views/Admin/Schedule/Index.cshtml", timeslot);
        }

    }
}

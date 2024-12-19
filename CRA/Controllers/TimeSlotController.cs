using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRA.Controllers
{
    public class TimeSlotController : Controller
    {
        private readonly ITimeSlotRepository _repository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IPeriodRepository _periodRepository;
        public TimeSlotController(ITimeSlotRepository repository, IAssignmentRepository assignmentRepository, IPeriodRepository periodRepository)
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
            _periodRepository = periodRepository;
        }
        public IActionResult Index()
        {
            IEnumerable<TimeSlot> timeSlots = _repository.GetAllTimeSlot();
            return View(timeSlots);
        }

        public IActionResult List(Guid code)
        {
            IEnumerable<TimeSlot> timeSlots = _repository.GetByAssignmentCode(code);

            @ViewData["code"] = code;
            return View(timeSlots);
        }

        public IActionResult AssignmentsList()
        {
            IEnumerable<Assignment> assignments = _assignmentRepository.GetAllAssignments();
            return View(assignments);
        }

        public IActionResult Create(Guid code)
        {
            @ViewData["code"] = code;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Guid code, TimeSlot timeSlot)
        {
            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound("Assignment not found.");
            }

            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            if (period == null)
            {
                return NotFound("Period not found.");
            }

            if (timeSlot.Start == default || timeSlot.End == default)
            {
                ModelState.AddModelError("", "Start and End dates must be valid.");
            }
            else if (timeSlot.Start >= period.Start && timeSlot.End <= period.End && timeSlot.Start < timeSlot.End)
            {
                if (ModelState.IsValid)
                {
                    timeSlot.AssignmentCode = assignment.Code;
                    _repository.AddTimeSlot(timeSlot);
                    ViewData["code"] = timeSlot.AssignmentCode;
                    return RedirectToAction(nameof(List), new {code = timeSlot.AssignmentCode});
                }
            }
            else
            {
                ModelState.AddModelError("", "The TimeSlot must fall within the assignment period and have valid start/end times.");
            }

            ViewData["code"] = code;
            return View(timeSlot);
        }


        public IActionResult Edit(Guid id)
        {
            var timeSlot = _repository.GetTimeSlotById(id);
            if (timeSlot == null)
            {
                return NotFound();
            }
            ViewData["code"] = timeSlot.AssignmentCode;
            return View(timeSlot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TimeSlot timeSlot)
        {
            var assignment = _assignmentRepository.GetAssignmentByCode(timeSlot.AssignmentCode);
            if (assignment == null)
            {
                return NotFound();
            }

            var period = _periodRepository.GetPeriodById(assignment.PeriodId);
            if (period == null)
            {
                return NotFound();
            }

            if (timeSlot.Start == default || timeSlot.End == default)
            {
                ModelState.AddModelError("", "Start and End dates must be valid.");
            }

            if (timeSlot.Start >= period.Start && timeSlot.End <= period.End && timeSlot.Start < timeSlot.End && timeSlot.End > timeSlot.Start)
            {
                if (ModelState.IsValid)
                {
                    _repository.UpdateTimeSlot(timeSlot);
                    ViewData["code"] = timeSlot.AssignmentCode;
                    return RedirectToAction(nameof(List), new { code = timeSlot.AssignmentCode });
                }
            }

            else
            {
                ModelState.AddModelError("", "The TimeSlot must fall within the assignment period and have valid start/end times.");
            }
            ViewData["code"] = timeSlot.AssignmentCode;
            return View(timeSlot);
        }

        public IActionResult Details(Guid id)
        {
            var timeSlot = _repository.GetTimeSlotById(id);
            if (timeSlot == null)
            {
                return NotFound();
            }
            ViewData["code"] = timeSlot.AssignmentCode;
            return View(timeSlot);
        }

        public IActionResult AssignmentsDetails(Guid code)
        {
            var assignment = _assignmentRepository.GetAssignmentByCode(code);
            if (assignment == null)
            {
                return NotFound();
            }
            @ViewData["code"] = code;
            return View(assignment);
        }

        public IActionResult Delete(Guid id)
        {
            var timeSlot = _repository.GetTimeSlotById(id);
            if (timeSlot == null)
            {
                return NotFound();
            }
            ViewData["code"] = timeSlot.AssignmentCode;
            return View(timeSlot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var timeSlot = _repository.GetTimeSlotById(id);
            _repository.DeleteTimeSlot(id);
            ViewData["code"] = timeSlot.AssignmentCode;
            return RedirectToAction(nameof(List), new { code = timeSlot.AssignmentCode });
        }
    }
}

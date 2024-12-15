using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRA.Controllers
{
    public class HomeEmployeeController : Controller
    {

        private readonly IEmployeeRepository _repository;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IAssignmentRepository _assignmentRepository;

        public HomeEmployeeController(IEmployeeRepository repository, IScheduleRepository scheduleRepository, IAssignmentRepository assignmentRepository)
        {
            _repository = repository;
            _scheduleRepository = scheduleRepository;
            _assignmentRepository = assignmentRepository;
        }

        public IActionResult Index(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = id;
            return View(employee);
        }

        public IActionResult Edit(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = id;
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateEmployee(employee);
                RedirectToAction(nameof(Details), new { id = employee.Id });
            }
            return View(employee);
        }

        public IActionResult Details(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = id;
            return View(employee);
        }

        public IActionResult Schedules(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var schedules = _scheduleRepository.GetByEmployeeId(id);
            ViewData["EmployeeId"] = id;
            return View(schedules);
        }

        public IActionResult Assignments(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var schedules = _scheduleRepository.GetByEmployeeId(id);
            IEnumerable<Assignment> assignments;
            List<Assignment> assignmentsList = new List<Assignment>();
            foreach (var schedule in schedules)
            {
                assignments = _assignmentRepository.GetByScheduleId(schedule.Id);
                assignmentsList.AddRange(assignments);
            }
            assignments = assignmentsList;
            ViewData["EmployeeId"] = id;
            return View(assignments);
        }


        public IActionResult Report(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var schedules = _scheduleRepository.GetByEmployeeId(id);
            IEnumerable<Assignment> assignments;
            List<Assignment> assignmentsList = new List<Assignment>();
            foreach (var schedule in schedules)
            {
                assignments = _assignmentRepository.GetByScheduleId(schedule.Id);
                assignmentsList.AddRange(assignments);
            }
            assignments = assignmentsList;
            ViewData["EmployeeId"] = id;
            return View(assignments);
        }

        public IActionResult Generate(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = id;
            return View();
        }


    }
}

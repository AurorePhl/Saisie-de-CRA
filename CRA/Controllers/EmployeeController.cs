using CRA.DataAccess;
using CRA.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRA.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _repository;
        public EmployeeController(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        public IActionResult Index()
        {
            IEnumerable<Employee> employees = _repository.GetAllEmployees();
            return View(employees);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _repository.AddEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public IActionResult Edit(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if(employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _repository.UpdateEmployee(employee);
                return RedirectToAction(nameof(Index));
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
            return View(employee);
        }

        public IActionResult Delete(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _repository.DeleteEmployee(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Schedule(Guid id)
        {
            var employee = _repository.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }
    }
}

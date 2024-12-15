using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context) 
        {
            _context = context; 
        }
        public void AddEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(Guid id)
        {
            var employee = _context.Employee.Find(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employee.ToList();
        }

        public Employee GetEmployeeById(Guid id)
        {
            return _context.Employee.Find(id);
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

    }
}

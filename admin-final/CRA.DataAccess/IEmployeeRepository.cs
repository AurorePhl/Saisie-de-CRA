using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetAllEmployees();
        Employee GetEmployeeById(Guid id);
        void AddEmployee(Employee employee); 
        void UpdateEmployee(Employee employee); 
        void DeleteEmployee(Guid id);
    }
}

using CRA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRA.DataAccess;



namespace CRA.DataAccess
{
    public class AssignmentRepository : IAssignmentRepository 
    {
        private readonly ApplicationDbContext _context;
        public AssignmentRepository(ApplicationDbContext context) // constructeur
        {
            _context = context; // initialisation de la variable _context en local = ApplicationDbContext
        }
        public IEnumerable<Assignment> GetAllAssignments()
        {
            return _context.Assignment.ToList(); // retourne tout les élements de la table Assignments
        }
        public void AddAssignment(Assignment assignment)
        {
            _context.Assignment.Add(assignment); // ajoute un assignment
            _context.SaveChanges(); // sauvegarde les changements
        }
        public void DeleteAssignment(Guid code)
        {
            var assignment = _context.Assignment.Find(code); // trouve un assignment par son code
            if (assignment != null)
            {
                _context.Assignment.Remove(assignment); // supprime un assignment
                _context.SaveChanges(); // sauvegarde les changements
            }
        }
        public Assignment GetAssignmentByCode(Guid code)
        {
            return _context.Assignment.Find(code);
        }
        public void UpdateAssignment(Assignment assignment) 
        {
            _context.Entry(assignment).State = EntityState.Modified; //
            _context.SaveChanges();
        }
        public IEnumerable<Assignment> GetByScheduleId(Guid scheduleId)
        {
            return _context.Assignment.Where(a => a.ScheduleId == scheduleId).ToList();
        }

        public IEnumerable<AssignmentViewModel> GetAllAssignmentsWithDetails()
        {
            var query = from assignment in _context.Assignment
                        join period in _context.Set<Period>() on assignment.PeriodId equals period.Id into periods
                        from period in periods.DefaultIfEmpty()
                        join schedule in _context.Set<Schedule>() on assignment.ScheduleId equals schedule.Id into schedules
                        from schedule in schedules.DefaultIfEmpty()
                        join employee in _context.Set<Employee>() on schedule.EmployeeId equals employee.Id into employees
                        from employee in employees.DefaultIfEmpty()
                        select new AssignmentViewModel
                        {
                            Code = assignment.Code,
                            Libelle = assignment.Libelle,
                            Description = assignment.Description,
                            Username = employee != null ? employee.Username : "no assignment",
                            Start = period != null ? period.Start : (DateTime?)null,
                            End = period != null ? period.End : (DateTime?)null
                        };

            return query.ToList();
        }
    }
}


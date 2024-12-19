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

        public AssignmentRepository(ApplicationDbContext context) // Constructeur
        {
            _context = context; // Initialisation de la variable _context en local = ApplicationDbContext
        }

        // Récupérer toutes les affectations
        public IEnumerable<Assignment> GetAllAssignments()
        {
            return _context.Assignment.ToList(); // Retourne tous les éléments de la table Assignments
        }

        // Ajouter une nouvelle affectation
        public void AddAssignment(Assignment assignment)
        {
            _context.Assignment.Add(assignment); // Ajoute une nouvelle affectation
            _context.SaveChanges(); // Sauvegarde les changements dans la base de données
        }

        // Supprimer une affectation par son code
        public void DeleteAssignment(Guid code)
        {
            var assignment = _context.Assignment.Find(code); // Trouve une affectation par son code
            if (assignment != null)
            {
                _context.Assignment.Remove(assignment); // Supprime l'affectation trouvée
                _context.SaveChanges(); // Sauvegarde les changements dans la base de données
            }
        }

        // Récupérer une affectation par son code
        public Assignment GetAssignmentByCode(Guid code)
        {
            return _context.Assignment.Find(code); // Trouve et retourne l'affectation par son code
        }

        // Mettre à jour une affectation existante
        public void UpdateAssignment(Assignment assignment)
        {
            _context.Entry(assignment).State = EntityState.Modified; // Met à jour l'état de l'affectation
            _context.SaveChanges(); // Sauvegarde les changements dans la base de données
        }

        // Récupérer les affectations par un identifiant de planning (ScheduleId)
        public IEnumerable<Assignment> GetByScheduleId(Guid scheduleId)
        {
            return _context.Assignment.Where(a => a.ScheduleId == scheduleId).ToList(); // Récupère les affectations selon le ScheduleId
        }

        // Récupérer une affectation par un identifiant de période (PeriodId)
        public Assignment GetByPeriodId(Guid periodId)
        {
            return _context.Assignment.FirstOrDefault(a => a.PeriodId == periodId); // Récupère la première affectation qui correspond au PeriodId
        }

        // Récupérer toutes les affectations avec les détails associés (Période, Planning, Employé)
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

            return query.ToList(); // Retourne la liste des AssignmentViewModels avec leurs détails
        }
    }
}

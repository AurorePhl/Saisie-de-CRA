using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public interface IAssignmentRepository
    {
        IEnumerable<Assignment> GetAllAssignments(); // Récupérer toutes les affectations
        Assignment GetAssignmentByCode(Guid code); // Récupérer une affectation par son code
        void AddAssignment(Assignment assignment); // Ajouter un assignment
        void UpdateAssignment(Assignment assignment); // Mettre à jour un assignment
        void DeleteAssignment(Guid code); // Supprimer un assignment
        IEnumerable<Assignment> GetByScheduleId(Guid scheduleId); // Récupérer les affectations par ScheduleId
        Assignment GetByPeriodId(Guid periodId); // Récupérer une affectation par PeriodId

        IEnumerable<AssignmentViewModel> GetAllAssignmentsWithDetails(); // Récupérer toutes les affectations avec les détails associés (Période, Planning, Employé)
    }
}

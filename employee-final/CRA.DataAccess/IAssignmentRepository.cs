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
        IEnumerable<Assignment> GetAllAssignments();
        Assignment GetAssignmentByCode(Guid code);
        void AddAssignment(Assignment assignment); // ajouter un assignment
        void UpdateAssignment(Assignment assignment); // mettre à jour un assignment
        void DeleteAssignment(Guid code); // supprimer un assignment
        //bool AssignmentExists(Guid code); // vérifier si un assignment existe
        //bool Save(); // sauvegarder les modifications
        IEnumerable<Assignment> GetByScheduleId(Guid scheduleId);
        Assignment GetByPeriodId(Guid periodId);
    }
}

using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public interface ITimeSlotRepository
    {
        IEnumerable<TimeSlot> GetAllTimeSlot();
        TimeSlot GetTimeSlotById(Guid id);
        void AddTimeSlot(TimeSlot timeSlot); 
        void UpdateTimeSlot(TimeSlot timeSlot);
        void DeleteTimeSlot(Guid id); 
        IEnumerable<TimeSlot> GetByAssignmentCode(Guid assignmentCode);
    }
}

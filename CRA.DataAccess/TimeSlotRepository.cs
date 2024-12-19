using CRA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class TimeSlotRepository : ITimeSlotRepository
    {
        private readonly ApplicationDbContext _context;
        public TimeSlotRepository(ApplicationDbContext context) // constructeur
        {
            _context = context; // initialisation de la variable _context en local = ApplicationDbContext
        }
        public void AddTimeSlot(TimeSlot timeSlot)
        {
            _context.TimeSlot.Add(timeSlot);
            _context.SaveChanges();
        }

        public void DeleteTimeSlot(Guid id)
        {
            var timeSlot = _context.TimeSlot.Find(id);
            if (timeSlot != null)
            {
                _context.TimeSlot.Remove(timeSlot);
                _context.SaveChanges();
            }
        }

        public IEnumerable<TimeSlot> GetAllTimeSlot()
        {
            return _context.TimeSlot.ToList();
        }

        public IEnumerable<TimeSlot> GetByAssignmentCode(Guid assignmentCode)
        {
            return _context.TimeSlot.Where(t => t.AssignmentCode == assignmentCode).ToList();
        }

        public TimeSlot GetTimeSlotById(Guid id)
        {
            return _context.TimeSlot.Find(id);
        }

        public void UpdateTimeSlot(TimeSlot timeSlot)
        {
            var existing = _context.TimeSlot.Find(timeSlot.Id);
            if (existing != null)
            {
                existing.State = timeSlot.State;
                _context.SaveChanges();
            }
            // _context.Entry(timeSlot).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public IEnumerable<SentTimeSlotViewModel> SentTimeSlots()
        {
            var query = from timeSlot in _context.TimeSlot
                        join assignment in _context.Assignment
                            on timeSlot.AssignmentCode equals assignment.Code
                        join schedule in _context.Schedule
                            on assignment.ScheduleId equals schedule.Id
                        join employee in _context.Employee
                            on schedule.EmployeeId equals employee.Id
                        where timeSlot.State == "sent" || timeSlot.State == "validated" || timeSlot.State == "rejected"
                        select new SentTimeSlotViewModel
                        {
                            Id = timeSlot.Id,
                            Start = timeSlot.Start,
                            End = timeSlot.End,
                            State = timeSlot.State,
                            AssignmentCode = timeSlot.AssignmentCode,
                            Libelle = assignment.Libelle,
                            Description = assignment.Description,
                            Username = employee.Username
                        };

            return query.ToList();
        }

    }
}

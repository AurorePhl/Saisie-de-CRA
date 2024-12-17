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
        public TimeSlot GetTimeSlotById(Guid timeslot)
        {
            return _context.TimeSlot.Find(timeslot);
        }
        public void UpdateTimeSlot(TimeSlot timeslot)
        {
            _context.Entry(timeslot).State = EntityState.Modified; //
            _context.SaveChanges();
        }

    }
}

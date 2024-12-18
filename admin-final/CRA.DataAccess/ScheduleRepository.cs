using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public ScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public void Delete(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Schedule> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Schedule> GetByEmployeeId(Guid employeeId)
        {
            return _context.Schedule.Where(s => s.EmployeeId == employeeId).ToList();
        }

        public Schedule GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Schedule schedule)
        {
            throw new NotImplementedException();
        }
    }
}

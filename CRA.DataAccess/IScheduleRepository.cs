using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public interface IScheduleRepository
    {
        IEnumerable<Schedule> GetAll();
        Schedule GetById(Guid id);
        IEnumerable<Schedule> GetByEmployeeId(Guid employeeId);
        void Add(Schedule schedule);
        void Update(Schedule schedule);
        void Delete(Schedule schedule);

    }
}

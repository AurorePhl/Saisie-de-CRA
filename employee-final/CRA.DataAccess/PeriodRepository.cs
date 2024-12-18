using CRA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class PeriodRepository : IPeriodRepository
    {
        private readonly ApplicationDbContext _context;

        public PeriodRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddPeriod(Period period)
        {
            throw new NotImplementedException();
        }

        public void DeletePeriod(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Period> GetAllPeriods()
        {
            return _context.Period.ToList();
        }

        public Period GetPeriodById(Guid id)
        {
            return _context.Period.Find(id);
        }

        public void UpdatePeriod(Period period)
        {
            throw new NotImplementedException();
        }
    }
}

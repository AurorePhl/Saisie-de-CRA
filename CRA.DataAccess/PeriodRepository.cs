using CRA.Models;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class PeriodRepository :IPeriodRepository
    {
        private readonly ApplicationDbContext _context;
        public PeriodRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Period> GetAllPeriods()
        {
            return _context.Period.ToList();
        }
    }
}

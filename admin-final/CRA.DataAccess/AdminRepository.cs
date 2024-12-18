using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class AdminRepository :IAdminRepository
    {
        private readonly ApplicationDbContext _context;
        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Admin> GetAllAdmins()
        {
            return _context.Admin.ToList();
        }
        public Admin GetAdminById(Guid id)
        {
            return _context.Find<Admin>(id);
        }
    }
}

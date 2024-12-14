using CRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class AssignmentRepository : IAssignmentRepository 
    {
        private readonly ApplicationDbContext _context;
        public AssignmentRepository(ApplicationDbContext context) // constructeur
        {
            _context = context; // initialisation de la variable _context en local = ApplicationDbContext
        }
        public IEnumerable<Assignment> GetAllAssignments()
        {
            return _context.Assignment.ToList(); // retourne tout les élements de la table Assignments
        }
        public void AddAssignment(Assignment assignment)
        {
            throw new NotImplementedException();
        }
        public void DeleteAssignment(Guid code)
        {
            throw new NotImplementedException();
        }
        public Assignment GetAssignmentByCode(Guid code)
        {
            throw new NotImplementedException();
        }
        public void UpdateAssignment(Assignment assignment)
        {
            throw new NotImplementedException();
        }
    }
}

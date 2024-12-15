using CRA.Models;
using Microsoft.EntityFrameworkCore;
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
            _context.Assignment.Add(assignment); // ajoute un assignment
            _context.SaveChanges(); // sauvegarde les changements
        }
        public void DeleteAssignment(Guid code)
        {
            var assignment = _context.Assignment.Find(code); // trouve un assignment par son code
            if (assignment != null)
            {
                _context.Assignment.Remove(assignment); // supprime un assignment
                _context.SaveChanges(); // sauvegarde les changements
            }
        }
        public Assignment GetAssignmentByCode(Guid code)
        {
            return _context.Assignment.Find(code);
        }
        public void UpdateAssignment(Assignment assignment) 
        {
            _context.Entry(assignment).State = EntityState.Modified; //
            _context.SaveChanges();
        }

    }
}

using CRA.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { } //  constructeur de la classe ApplicationDbContext 

        public DbSet<Assignment> Assignment { get; set; } // DbSet est une collection d'entités qui peuvent être ajoutées, mises à jour et supprimées de la base de données

    }
}

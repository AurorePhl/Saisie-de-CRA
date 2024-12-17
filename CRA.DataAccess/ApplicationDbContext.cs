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

        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Period> Period { get; set; }
        public DbSet<TimeSlot> TimeSlot { get; set; }
    }
}

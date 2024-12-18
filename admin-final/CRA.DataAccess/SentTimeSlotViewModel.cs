using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.DataAccess
{
   
    public class SentTimeSlotViewModel
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string State { get; set; }
        public Guid AssignmentCode { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
    }
    
}

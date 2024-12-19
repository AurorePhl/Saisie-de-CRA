using System.Runtime.CompilerServices;

namespace CRA.Models
{
    public class TimeSlotViewModel
    {
        public Guid Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public String State { get; set; }
        public string AssignmentLibelle { get; set; }

        public Guid AssignmentCode { get; set; }
        public List<Guid> AssignmentsCodeList { get; set; }
        public List<String> AssignmentsLibelleList { get; set; }

    }
}

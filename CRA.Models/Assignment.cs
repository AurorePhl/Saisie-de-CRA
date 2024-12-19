using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Models
{
    public class Assignment
    {
        [Key]
        public Guid Code { get; set; }

        [Required]
        [MaxLength(200)]
        public string Libelle { get; set; }

        public string Description { get; set; }

        public bool? IsValidated { get; set; }
        public bool? IsAssigned { get; set; }

        public Guid? ScheduleId { get; set; }

        public Guid? AdminId { get; set; }
        public Guid? PeriodId { get; set; }



    }
}

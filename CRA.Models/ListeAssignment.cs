using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Models
{
    public class ListeAssignment
    {
        [Key]
        public Guid Code { get; set; }

        [Required(ErrorMessage = "Libellé obligatoire")]
        [MaxLength(200, ErrorMessage = "Libellé trop long")]
        public string Libelle { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsValidated { get; set; }

        [Required]
        public bool IsAssigned { get; set; }

        public Guid? ScheduleId { get; set; }

        public Guid? AdminId { get; set; }

        [Required]
        public Guid PeriodId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRA.Models
{
    public class Schedule
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public bool IsCopied { get; set; }

        [Required]
        public bool IsSaved { get; set; }

        [Required]
        public bool IsSent { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

    }
}

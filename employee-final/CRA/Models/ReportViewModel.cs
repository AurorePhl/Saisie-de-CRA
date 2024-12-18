using Microsoft.AspNetCore.Mvc;

namespace CRA.Models
{
    public class ReportViewModel
    {
        public string EmployeeName { get; set; } // Nom de l'employé
        public string Month { get; set; }           // Mois du rapport
        public string Year { get; set; }            // Année du rapport
        public List<AssignmentReportItem> Assignments { get; set; } // Liste des missions
    }

    public class AssignmentReportItem
    {
        public Guid Code { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public double TotalWorkedDays { get; set; } // Nombre total d'heures travaillées
    }

}

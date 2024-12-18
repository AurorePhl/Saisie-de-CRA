namespace CRA.DataAccess
{
    public class AssignmentViewModel
    {
        public Guid Code { get; set; }
        public string Libelle { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}
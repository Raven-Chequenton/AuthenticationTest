namespace AuthenticationTest.Models.ViewModels
{
    public class TicketViewModel
    {
        public int Id { get; set; } // ✅ Add Ticket ID

        public string TicketRef { get; set; }
        public string RequestorEmail { get; set; }
        public string ShortDescription { get; set; }
        public string CompanyName { get; set; } // ✅ New Field for Display
        public int? DepartmentId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}

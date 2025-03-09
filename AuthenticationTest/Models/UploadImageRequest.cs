namespace AuthenticationTest.Models
{
    public class UploadImageRequest
    {
        public int TicketId { get; set; }
        public string Image { get; set; }
        public string Type { get; set; } // "internalNote" or "customerComm"
    }
}

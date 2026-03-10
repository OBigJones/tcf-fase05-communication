namespace Communication.Api.Contracts.Requests
{
    public class SendCommunicationRequest
    {
        public string Email { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // "Success" or "Failure"
    }
}

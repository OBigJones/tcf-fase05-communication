namespace Communication.Api.Contracts.Responses
{
    public class SendCommunicationResponse
    {
        public bool Sent { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

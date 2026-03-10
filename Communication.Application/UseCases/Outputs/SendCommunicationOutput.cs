namespace Communication.Application.UseCases.Outputs
{
    public class SendCommunicationOutput
    {
        public bool Sent { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

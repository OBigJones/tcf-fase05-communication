namespace Communication.Application.UseCases.Inputs
{
    public class SendCommunicationInput
    {
        public string Email { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}

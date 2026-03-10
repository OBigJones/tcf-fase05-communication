namespace Communication.Infrastructure.Messaging
{
    public class VideoProcessingResultMessage
    {
        public string Email { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}

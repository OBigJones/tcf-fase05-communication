namespace Communication.Domain.ValueObjects
{
    public class VideoProcessingNotificationData
    {
        public string Email { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}

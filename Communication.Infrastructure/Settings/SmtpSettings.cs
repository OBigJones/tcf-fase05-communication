namespace Communication.Infrastructure.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 25;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string From { get; set; } = "no-reply@communication.local";
    }
}

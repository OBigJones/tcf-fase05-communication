using System;

namespace Communication.Domain.ValueObjects
{
    public class EmailMessage
    {
        public string To { get; }
        public string From { get; }
        public string Subject { get; }
        public string Body { get; }

        public EmailMessage(string to, string from, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentException("Recipient email is required", nameof(to));
            if (string.IsNullOrWhiteSpace(from)) throw new ArgumentException("From email is required", nameof(from));
            To = to;
            From = from;
            Subject = subject ?? string.Empty;
            Body = body ?? string.Empty;
        }
    }
}

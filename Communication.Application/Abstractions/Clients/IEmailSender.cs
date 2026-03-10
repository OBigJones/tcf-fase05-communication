using System.Threading.Tasks;
using Communication.Domain.ValueObjects;

namespace Communication.Application.Abstractions.Clients
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}

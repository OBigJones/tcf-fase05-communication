using System.Threading.Tasks;
using Communication.Application.UseCases.Inputs;

namespace Communication.Application.Abstractions.Messaging
{
    public interface ICommunicationQueueConsumer
    {
        Task StartAsync();
        Task StopAsync();
    }
}

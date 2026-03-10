using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Communication.Infrastructure.Messaging
{
    public class RabbitMqConsumerHostedService : IHostedService
    {
        private readonly RabbitMqConsumer _consumer;
        private readonly ILogger<RabbitMqConsumerHostedService> _logger;

        public RabbitMqConsumerHostedService(RabbitMqConsumer consumer, ILogger<RabbitMqConsumerHostedService> logger)
        {
            _consumer = consumer;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting RabbitMQ consumer hosted service");
            try
            {
                await _consumer.StartAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to start RabbitMQ consumer");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RabbitMQ consumer hosted service");
            await _consumer.StopAsync();
        }
    }
}

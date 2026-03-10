using Communication.Application.UseCases.Handlers;
using Communication.Application.UseCases.Inputs;
using Communication.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Communication.Infrastructure.Messaging
{
    public class RabbitMqConsumer
    {
        private readonly RabbitMqSettings _settings;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;
        private string? _consumerTag;
        private readonly SendCommunicationHandler _handler;

        public RabbitMqConsumer(IOptions<RabbitMqSettings> settings, ILogger<RabbitMqConsumer> logger, SendCommunicationHandler handler)
        {
            _settings = settings.Value;
            _logger = logger;
            _handler = handler;
            _factory = new ConnectionFactory
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };
        }

        public async Task StartAsync()
        {
            try
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync(); // v7 usa CreateChannelAsync

                await _channel.QueueDeclareAsync(
                    queue: _settings.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += OnMessageReceivedAsync; // v7 usa ReceivedAsync

                _consumerTag = await _channel.BasicConsumeAsync(
                    queue: _settings.QueueName,
                    autoAck: false,
                    consumer: consumer);

                _logger.LogInformation("RabbitMQ consumer started on queue {Queue}", _settings.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to start RabbitMQ consumer");
            }
        }

        public async Task StopAsync()
        {
            try
            {
                if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
                {
                    try
                    {
                        await _channel.BasicCancelAsync(_consumerTag);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to cancel consumer tag {Tag}", _consumerTag);
                    }
                }

                if (_channel != null)
                {
                    await _channel.CloseAsync();
                    _channel.Dispose();
                    _channel = null;
                }

                if (_connection != null)
                {
                    await _connection.CloseAsync();
                    _connection.Dispose();
                    _connection = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while stopping RabbitMQ consumer");
            }
        }

        // Assinatura correta para o ReceivedAsync
        private async Task OnMessageReceivedAsync(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation("Message received: {Message}", message);

            try
            {
                var dto = JsonSerializer.Deserialize<VideoProcessingResultMessage>(message, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (dto == null)
                {
                    _logger.LogWarning("Message deserialized to null");
                    if (_channel != null) await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    return;
                }

                var input = new SendCommunicationInput
                {
                    Email = dto.Email,
                    FileName = dto.FileName,
                    Success = dto.Success
                };

                await _handler.HandleAsync(input);

                if (_channel != null)
                {
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                    _logger.LogInformation("Message processed and acked");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message");
                if (_channel != null)
                {
                    // Em erro, damos Ack para não travar a fila (poison message), 
                    // mas o ideal futuro é enviar para uma Dead Letter Queue (DLQ)
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                }
            }
        }
    }
}
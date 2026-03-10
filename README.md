# Communication microservice

Microserviço responsável por consumir mensagens de processamento de vídeo via RabbitMQ e enviar comunicações por email via SMTP.

Arquitetura: Clean/Onion - Domain, Application, Infrastructure, API.

Fluxo:
- RabbitMQ -> Consumer (Infrastructure) -> Application Handler -> Domain templates -> SMTP Sender (Infrastructure)

Endpoint manual para testes:
POST /communications/test
Payload:
{
  "email": "cliente@teste.com",
  "fileName": "video-processado.mp4",
  "status": "Success"
}

Configurações em appsettings.json:
- RabbitMq: HostName, Port, UserName, Password, QueueName
- Smtp: Host, Port, User, Password, From, EnableSsl

Executar:
- dotnet run no projeto Communication.Api
- Verificar Swagger na raiz para testar endpoint
- Para testes end-to-end, publicar message na fila configurada
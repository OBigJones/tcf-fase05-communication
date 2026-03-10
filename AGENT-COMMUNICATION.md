# Diretrizes de implementação - Microserviço Communication

## Objetivo
Implementar integralmente o microserviço `Communication` seguindo Clean Architecture, Onion Architecture e princípios SOLID.

O serviço será responsável por consumir mensagens de uma fila RabbitMQ contendo:
- email do cliente
- nome do arquivo de vídeo
- resultado do processamento

Com base nesse evento, o serviço deverá enviar um email apropriado:
- em caso de sucesso: email informando que o resultado está pronto para download
- em caso de falha: email informando que houve falha no processamento

## Regras gerais
- Não pedir confirmação ao usuário durante a implementação.
- Não interromper para perguntar detalhes menores.
- Assumir decisões simples e sensatas quando houver ambiguidade pequena.
- Manter o projeto pequeno, limpo, coeso e pronto para testes manuais.
- Respeitar estritamente a separação de responsabilidades por camada.
- Não colocar regra de negócio na camada API.
- Não colocar acesso a infraestrutura na camada Domain.
- A Application deve orquestrar o fluxo e depender apenas de abstrações.
- A Infrastructure deve implementar as abstrações.

## Arquitetura esperada

### API
Responsável apenas por:
- expor endpoint manual para testes
- configurar Swagger
- configurar DI
- mapear request/response
- hospedar a aplicação

### Application
Responsável por:
- DTOs de entrada e saída
- handlers
- interfaces de clientes externos
- interfaces de mensageria
- orquestração do fluxo de envio de email

### Domain
Responsável por:
- enum de templates
- classes de template
- objetos de valor
- regras de composição do email
- exceções de domínio

### Infrastructure
Responsável por:
- consumer RabbitMQ
- hosted service do consumer
- implementação de envio de email via SMTP
- settings e configuração
- DI de infraestrutura

## Fluxo principal
1. O consumer RabbitMQ recebe uma mensagem.
2. A mensagem contém email do cliente, nome do arquivo e status do processamento.
3. A Infrastructure converte a mensagem para um DTO da Application.
4. A Application chama o handler de envio de comunicação.
5. O handler resolve o template correto.
6. O handler monta o email final.
7. O handler chama `IEmailSender`.
8. O serviço de infraestrutura envia o email.
9. O consumer registra logs e confirma a mensagem.

## Templates
Criar dois templates de email:
- Success
- Failure

### Success
Deve informar:
- nome do arquivo
- processamento concluído com sucesso
- resultado disponível para download

### Failure
Deve informar:
- nome do arquivo
- falha no processamento
- orientação amigável para tentar novamente depois ou contatar suporte

## Modelagem obrigatória

### Domain
Criar:
- `CommunicationTemplateType`
- `CommunicationTemplate`
- `SuccessCommunicationTemplate`
- `FailureCommunicationTemplate`
- `EmailMessage`
- `VideoProcessingNotificationData`
- `InvalidCommunicationException`

### Application
Criar:
- `SendCommunicationInput`
- `SendCommunicationOutput`
- `SendCommunicationHandler`
- `IEmailSender`
- `ICommunicationQueueConsumer`
- `IMessageProcessor`

### Infrastructure
Criar:
- `SmtpEmailSender`
- `RabbitMqConsumer`
- `RabbitMqConsumerHostedService`
- `VideoProcessingResultMessage`
- `RabbitMqSettings`
- `SmtpSettings`
- `InfrastructureDependencyInjection`

### API
Criar:
- `SendCommunicationRequest`
- `SendCommunicationResponse`
- `CommunicationMapper`
- `CommunicationController`

## Endpoint manual
Criar endpoint:
- `POST /communications/test`

Esse endpoint deve:
- receber email, nome do arquivo e status
- chamar o mesmo fluxo do handler principal
- retornar resultado objetivo para teste manual

## Restrições e boas práticas
- Usar async/await corretamente.
- Validar entrada no handler.
- Usar `ILogger` nas camadas apropriadas.
- Não usar lógica estática espalhada.
- Preferir classes pequenas e coesas.
- Nomear métodos e classes de forma explícita.
- Escrever código pronto para manutenção.
- Adicionar comentários apenas quando realmente úteis.
- Não gerar código morto.
- Não criar complexidade desnecessária.

## Mensageria
Implementar consumo RabbitMQ de forma simples e funcional:
- ler fila configurada via `appsettings`
- desserializar JSON da mensagem
- converter para input da Application
- processar
- fazer ack em sucesso
- em erro, registrar log e fazer tratamento simples sem travar o serviço

## Email
Implementar envio SMTP configurável via `appsettings`.
O email deve ter:
- destinatário
- assunto
- corpo textual claro
- remetente configurável

## Configuração
Adicionar seções no `appsettings.json`:
- `RabbitMq`
- `Smtp`

## Testes mínimos
Criar testes para:
- handler com template de sucesso
- handler com template de falha
- input inválido
- controller chamando o handler

## Entrega esperada
Ao final, o projeto deve:
- compilar
- subir normalmente
- expor Swagger
- aceitar teste manual via endpoint
- iniciar o consumer RabbitMQ
- conseguir enviar emails com base em mensagens recebidas
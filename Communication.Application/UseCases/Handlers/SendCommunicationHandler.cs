using System;
using System.Threading.Tasks;
using Communication.Application.UseCases.Inputs;
using Communication.Application.UseCases.Outputs;
using Communication.Application.Abstractions.Clients;
using Communication.Domain.Entities;
using Communication.Domain.Enums;
using Communication.Domain.Exceptions;

namespace Communication.Application.UseCases.Handlers
{
    public class SendCommunicationHandler
    {
        private readonly IEmailSender _emailSender;

        public SendCommunicationHandler(IEmailSender emailSender)
        {
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        public async Task<SendCommunicationOutput> HandleAsync(SendCommunicationInput input)
        {
            if (input == null) throw new InvalidCommunicationException("Input is required");
            if (string.IsNullOrWhiteSpace(input.Email)) throw new InvalidCommunicationException("Email is required");
            if (string.IsNullOrWhiteSpace(input.FileName)) throw new InvalidCommunicationException("FileName is required");

            CommunicationTemplate template = input.Success
                ? new SuccessCommunicationTemplate()
                : new FailureCommunicationTemplate();

            template.BuildBody(input.FileName);

            var email = new Communication.Domain.ValueObjects.EmailMessage(
                input.Email,
                "no-reply@communication.local",
                template.Subject,
                template.Body);

            await _emailSender.SendAsync(email);

            return new SendCommunicationOutput { Sent = true, Message = "Email sent" };
        }
    }
}

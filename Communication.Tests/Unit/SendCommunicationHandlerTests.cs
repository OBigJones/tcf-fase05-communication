using Xunit;
using Moq;
using FluentAssertions;
using System.Threading.Tasks;
using Communication.Application.UseCases.Handlers;
using Communication.Application.UseCases.Inputs;
using Communication.Application.Abstractions.Clients;
using Communication.Domain.ValueObjects;

namespace Communication.Tests.Unit
{
    public class SendCommunicationHandlerTests
    {
        [Fact]
        public async Task HandleAsync_WhenSuccess_ShouldSendEmail()
        {
            var emailSenderMock = new Mock<IEmailSender>();
            emailSenderMock.Setup(x => x.SendAsync(It.IsAny<EmailMessage>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new SendCommunicationHandler(emailSenderMock.Object);

            var input = new SendCommunicationInput { Email = "test@local", FileName = "file.mp4", Success = true };

            var output = await handler.HandleAsync(input);

            output.Sent.Should().BeTrue();
            emailSenderMock.Verify();
        }

        [Fact]
        public async Task HandleAsync_WhenFailure_ShouldSendEmail()
        {
            var emailSenderMock = new Mock<IEmailSender>();
            emailSenderMock.Setup(x => x.SendAsync(It.IsAny<EmailMessage>())).Returns(Task.CompletedTask).Verifiable();

            var handler = new SendCommunicationHandler(emailSenderMock.Object);

            var input = new SendCommunicationInput { Email = "test@local", FileName = "file.mp4", Success = false };

            var output = await handler.HandleAsync(input);

            output.Sent.Should().BeTrue();
            emailSenderMock.Verify();
        }

        [Fact]
        public async Task HandleAsync_WhenInvalidInput_ShouldThrow()
        {
            var emailSenderMock = new Mock<IEmailSender>();
            var handler = new SendCommunicationHandler(emailSenderMock.Object);

            await Assert.ThrowsAsync<System.Exception>(async () => await handler.HandleAsync(null!));
        }
    }
}

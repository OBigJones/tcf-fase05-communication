using Communication.Api.Contracts.Requests;
using Communication.Application.UseCases.Inputs;

namespace Communication.Api.Mappers
{
    public static class CommunicationMapper
    {
        public static SendCommunicationInput ToInput(this SendCommunicationRequest request)
        {
            return new SendCommunicationInput
            {
                Email = request.Email,
                FileName = request.FileName,
                Success = string.Equals(request.Status, "Finished", System.StringComparison.OrdinalIgnoreCase)
            };
        }
    }
}

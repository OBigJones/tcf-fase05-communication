using Communication.Domain.Enums;

namespace Communication.Domain.Entities
{
    public class FailureCommunicationTemplate : CommunicationTemplate
    {
        public FailureCommunicationTemplate() : base(CommunicationTemplateType.Failure)
        {
            Subject = "Falha no processamento do seu vídeo";
        }

        public override void BuildBody(string fileName)
        {
            Body = $"Olá,\n\nO processamento do arquivo de vídeo '{fileName}' falhou. Por favor, tente novamente mais tarde ou contacte o suporte.\n\nAtenciosamente,\nEquipe de Processamento";
        }
    }
}

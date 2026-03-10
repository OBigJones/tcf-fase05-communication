using Communication.Domain.Enums;

namespace Communication.Domain.Entities
{
    public class SuccessCommunicationTemplate : CommunicationTemplate
    {
        public SuccessCommunicationTemplate() : base(CommunicationTemplateType.Success)
        {
            Subject = "Processamento concluído: seu vídeo está disponível";
        }

        public override void BuildBody(string fileName)
        {
            Body = $"Olá,\n\nSeu arquivo de vídeo '{fileName}' foi processado com sucesso. O resultado está disponível para download.\n\nAtenciosamente,\nEquipe de Processamento";
        }
    }
}

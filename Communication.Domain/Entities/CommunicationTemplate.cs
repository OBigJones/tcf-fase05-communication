using Communication.Domain.Enums;

namespace Communication.Domain.Entities
{
    public abstract class CommunicationTemplate
    {
        public CommunicationTemplateType TemplateType { get; }
        public string Subject { get; protected set; } = string.Empty;
        public string Body { get; protected set; } = string.Empty;

        protected CommunicationTemplate(CommunicationTemplateType templateType)
        {
            TemplateType = templateType;
        }

        public abstract void BuildBody(string fileName);
    }
}

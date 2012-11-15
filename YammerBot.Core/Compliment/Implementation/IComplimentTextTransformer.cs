using YammerBot.Core.Compliment.Interface;

namespace YammerBot.Core.Compliment.Implementation
{
    public class ComplimentTextTransformer : IComplimentTextTransformer
    {
        public string TransformComplimentTextToJsonArrayText(string text)
        {
            return text.Replace("var compliments = ", "").Trim().TrimEnd(';');
        }
    }
}

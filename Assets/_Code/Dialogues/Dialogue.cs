namespace Dialogues
{
    public class Dialogue
    {
        public DialogueExcerpt Excerpt { get; }
        public DialogueSentence Response { get; }
        public DialogueBias Bias { get; }

        public Dialogue(DialogueExcerpt excerpt, DialogueSentence response)
        {
            Excerpt = excerpt;
            Response = response;

            Bias = new DialogueBias
            {
                poor = excerpt.Item2.poor * response.Biases.poor,
                rich = excerpt.Item2.rich * response.Biases.rich
            };
        }
    }
}
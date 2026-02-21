using System;

namespace Dialogues
{
    public class DialogueExcerpt : Tuple<string, DialogueBias>
    {
        public DialogueExcerpt(string filledText, DialogueBias bias) : base(filledText, bias)
        {
        }
    }
}

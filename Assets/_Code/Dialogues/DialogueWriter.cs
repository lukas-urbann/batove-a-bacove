using UnityEngine;

namespace Dialogues
{
    public class DialogueWriter : MonoBehaviour
    {
        public TypeEffect typeEffect;

        public void WriteSentence(string sentence)
        {
            typeEffect.ClearText();
            typeEffect.StartTyping(sentence);
        }
    }
}

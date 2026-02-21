using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "DialogueFillWord", menuName = "Dialogue Fill Word", order = 3)]
    public class DialogueFillWord : ScriptableObject
    {
        public DialogueWeight Weight;
        public DialogueTag[] Tags;
    }
}

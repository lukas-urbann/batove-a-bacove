using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "DialogueTag", menuName = "Dialogue Tag", order = 2)]
    public class DialogueTag : ScriptableObject
    {
        // jenom pro debug
        [TextArea]
        public string note;
    }
}
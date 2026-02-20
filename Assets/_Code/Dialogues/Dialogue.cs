using System.Collections.Generic;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue", order = 1)]
    public class Dialogue : ScriptableObject
    {
        public string characterName;
        public string mainDialogue;

        public void EvaluateResponseType(ResponseType type)
        {
            
        }
    }
}

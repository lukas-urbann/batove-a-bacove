using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class DialogueBias
    {
        [Range(-0.3f, 0.3f)] public float poor;
        [Range(-0.3f, 0.3f)] public float rich;
    }
}

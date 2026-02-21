using System;
using UnityEngine;

namespace Dialogues
{
    [Serializable]
    public class DialogueBias
    {
        [Range(-1, 1)] public float poor;
        [Range(-1, 1)] public float rich;
    }
}

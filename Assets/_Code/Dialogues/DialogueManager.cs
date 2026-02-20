using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dialogues
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }
        
        private IEnumerable<DialogueFillWord> _words;
        private IEnumerable<DialogueSentence> _indictments;
        
        public IEnumerable<DialogueFillWord> Words => _words;
        public IEnumerable<DialogueSentence> Indictments => _indictments;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            
            InitDialogueData();
        }

        public void Update()
        {
            //try random
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                var l = _indictments.ToList();
                var randomItem = l[UnityEngine.Random.Range(0, l.Count)];
                Debug.Log(randomItem.GetRandomizedText().Item1 + " | Weight:" + randomItem.GetRandomizedText().Item2.poor + " | " + randomItem.GetRandomizedText().Item2.rich);
            }
        }

        private void InitDialogueData()
        {
            _words = Resources.LoadAll("Words", typeof(DialogueFillWord)).Cast<DialogueFillWord>();
            _indictments = Resources.LoadAll("Indictments", typeof(DialogueSentence)).Cast<DialogueSentence>();
        }
    }
}

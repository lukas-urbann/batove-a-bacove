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
        private IEnumerable<DialogueSentence> _richIndictments;
        private IEnumerable<DialogueSentence> _poorIndictments;
        private IEnumerable<DialogueSentence> _responses;
        
        public IEnumerable<DialogueFillWord> Words => _words;
        public IEnumerable<DialogueSentence> RichIndictments => _richIndictments;
        public IEnumerable<DialogueSentence> PoorIndictments => _poorIndictments;
        public IEnumerable<DialogueSentence> Responses => _responses;

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

        private DialogueExcerpt GetRandomIndictment()
        {
            var l = RichIndictments.ToList();
            l.AddRange(PoorIndictments);
            var randomItem = l[UnityEngine.Random.Range(0, l.Count)];

            DialogueExcerpt dialogueExcerpt = randomItem.CreateDialogueExcerpt();
            return dialogueExcerpt;
        }
        
        private DialogueSentence GetRandomResponse()
        {
            var l = Responses.ToList();
            return l[UnityEngine.Random.Range(0, l.Count)];
        }

        private void InitDialogueData()
        {
            _words = Resources.LoadAll("Words", typeof(DialogueFillWord)).Cast<DialogueFillWord>();
            _richIndictments = Resources.LoadAll("Indictments/Rich", typeof(DialogueSentence)).Cast<DialogueSentence>();
            _poorIndictments = Resources.LoadAll("Indictments/Poor", typeof(DialogueSentence)).Cast<DialogueSentence>();
            _responses = Resources.LoadAll("Responses", typeof(DialogueSentence)).Cast<DialogueSentence>();
        }

        public void CreateNewDialogue()
        {
            Dialogue d = new Dialogue(GetRandomIndictment(), GetRandomResponse());
        }
    }
}

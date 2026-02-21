using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

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

        private DialogueExcerpt GetTypedIndictment(JudgedType judgedType)
        {
            return judgedType switch
            {
                JudgedType.Poor => GetRandomIndictment(PoorIndictments.ToList()),
                JudgedType.Rich => GetRandomIndictment(RichIndictments.ToList()),
                _ => GetRandomIndictment()
            };
        }
        
        private DialogueExcerpt GetRandomIndictment(List<DialogueSentence> l = null)
        {
            var randomItem = l?[Random.Range(0, l.Count)];
            Debug.Assert(randomItem != null, nameof(randomItem) + " != null");
            DialogueExcerpt dialogueExcerpt = randomItem.CreateDialogueExcerpt();
            return dialogueExcerpt;
        }
        
        private DialogueSentence GetRandomResponse()
        {
            var l = Responses.ToList();
            return l[Random.Range(0, l.Count)];
        }

        private void InitDialogueData()
        {
            _words = Resources.LoadAll("Words", typeof(DialogueFillWord)).Cast<DialogueFillWord>();
            _richIndictments = Resources.LoadAll("Indictments/Rich", typeof(DialogueSentence)).Cast<DialogueSentence>();
            _poorIndictments = Resources.LoadAll("Indictments/Poor", typeof(DialogueSentence)).Cast<DialogueSentence>();
            _responses = Resources.LoadAll("Responses", typeof(DialogueSentence)).Cast<DialogueSentence>();
        }

        public Dialogue CreateNewDialogue(JudgedType judgedType)
        {
            Dialogue d = new Dialogue(GetTypedIndictment(judgedType), GetRandomResponse());
            return d;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dialogues
{
    [CreateAssetMenu(fileName = "DialogueSentence", menuName = "Dialogue Sentence", order = 4)]
    public class DialogueSentence : DialogueText
    {
        public List<FillerWords> FillerWordsList = new();
        public DialogueBias Biases;

        [Serializable]
        public class FillerWords
        {
            public DialogueTag[] possibleFillTags;
        }

        public Tuple<string, DialogueBias> GetRandomizedText()
        {
            DialogueBias bias = new DialogueBias
            {
                poor = Biases.poor,
                rich = Biases.rich
            };
            
            string filled = ResolveTemplate(text, i =>
            {
                var tag = FillerWordsList[i].possibleFillTags[UnityEngine.Random.Range(0, FillerWordsList[i].possibleFillTags.Length)];
                var randomWord = RandomWord(DialogueManager.Instance.Words, w => w.Tags.Contains(tag));
                bias.poor += 1 * randomWord.Weight.weight;
                bias.rich += 1 * randomWord.Weight.weight;
                return randomWord.name;
            });
            
            return new Tuple<string, DialogueBias>(filled, bias);
        }
        
        public DialogueFillWord RandomWord(IEnumerable<DialogueFillWord> source, Func<DialogueFillWord, bool> predicate)
        {
            var filtered = source.Where(predicate).ToList();
            return filtered.Count == 0 ? null : filtered[UnityEngine.Random.Range(0, filtered.Count)];
        }

        // moje mylna predstava ze by tento moderni jazyk umel pracovat s argumenty byla velmi daleko od pravdy
        public string ResolveTemplate(string template, Func<int, string> resolver)
        {
            //dekuji c#
            return Regex.Replace(template, @"\{arg(\d+)\}", match =>
            {
                int index = int.Parse(match.Groups[1].Value);
                return resolver(index);
            });
        }
    }
}

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

        public DialogueExcerpt CreateDialogueExcerpt()
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
                bias.poor *= randomWord.Weight.weight;
                bias.rich *= randomWord.Weight.weight;
                return $"<i>{randomWord.name}</i>";
            });

            return new DialogueExcerpt(filled, bias);
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
            return Regex.Replace(template, @"\{(\d+)\}", match =>
            {
                //to cislovani tu je kvuli tomu ze pak muzeme dosadit slova se stejnymi tagy do textu nekolikrat
                //aniz by bylo potreba pro kazdy argument vybirat kategorie jednotlive
                int index = int.Parse(match.Groups[1].Value);
                return resolver(index);
            });
        }
    }
}

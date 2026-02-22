using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class RandomizeJudgedImage : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite[] poorSprites;
        [SerializeField] private Sprite[] richSprites;
        
        private Queue<Sprite> _poorQueue = new Queue<Sprite>();
        private Queue<Sprite> _richQueue = new Queue<Sprite>();

        private void Start()
        {
            GameManager.Instance.newJudgedTypeReady?.AddListener(Randomize);
        }

        private void OnDisable()
        {
            GameManager.Instance.newJudgedTypeReady?.RemoveListener(Randomize);
        }
        
        private Queue<Sprite> BuildQueue(Sprite[] sprites)
        {
            return new Queue<Sprite>(sprites.OrderBy(_ => Random.value));
        }

        private void Randomize(JudgedType judgedType)
        {
            switch (judgedType)
            {
                case JudgedType.Poor:
                    if (_poorQueue.Count == 0) _poorQueue = BuildQueue(poorSprites);
                    image.sprite = _poorQueue.Dequeue();
                    break;
                case JudgedType.Rich:
                    if (_richQueue.Count == 0) _richQueue = BuildQueue(richSprites);
                    image.sprite = _richQueue.Dequeue();
                    break;
            }
        }
    }
}


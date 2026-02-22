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

        private void Start()
        {
            GameManager.Instance.newJudgedTypeReady?.AddListener(Randomize);
        }

        private void OnDisable()
        {
            GameManager.Instance.newJudgedTypeReady?.RemoveListener(Randomize);
        }
        
        private void Randomize(JudgedType judgedType)
        {
            switch (judgedType)
            {
                case JudgedType.Poor:
                    image.sprite = poorSprites[Random.Range(0, poorSprites.Length)];
                    break;
                case JudgedType.Rich:
                    image.sprite = richSprites[Random.Range(0, richSprites.Length)];
                    break;
            }
        }
    }
}


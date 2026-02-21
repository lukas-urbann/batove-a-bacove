using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class RandomizeImage : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite[] sprites;

        private void Start()
        {
            Randomize();
        }

        private void Randomize()
        {
            image.sprite = sprites[Random.Range(0, sprites.Length)];
        }
    }
}


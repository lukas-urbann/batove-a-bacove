using Controllers;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class PauseScreen : MonoBehaviour
    {
        public UnityEvent OnPause;
        public UnityEvent OnResume;
        
        private void Start()
        {
            GameManager.Instance.OnGameResume += OnResume.Invoke;
            GameManager.Instance.OnGamePause += OnPause.Invoke;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGameResume -= OnResume.Invoke;
            GameManager.Instance.OnGamePause -= OnPause.Invoke;
        }
    }
}


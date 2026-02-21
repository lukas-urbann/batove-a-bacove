using System;
using Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI
{
    public class PauseScreen : MonoBehaviour
    {
        public UnityEvent OnPause;
        public UnityEvent OnResume;

        private void Update()
        {
            //nema to cenu, puvodne z ui control
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                GameManager.Instance.OnGamePause?.Invoke();
                Time.timeScale = 0;
            }
        }

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


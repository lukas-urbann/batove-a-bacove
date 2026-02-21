using Controllers;
using UnityEngine;

namespace UI
{
    public class UIControls : MonoBehaviour
    {
        public void ExitGame()
        {
            Application.Quit();
        }
        
        public void MoveToMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        
        public void MoveToGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
        
        public void ResumeGame()
        {
            GameManager.Instance.OnGameResume?.Invoke();
            Time.timeScale = 1;
        }

        public void PauseGame()
        {
            GameManager.Instance.OnGamePause?.Invoke();
            Time.timeScale = 0;
        }
    }
}


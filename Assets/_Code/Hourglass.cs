using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class Hourglass : MonoBehaviour
{
    public Image hourglassFillImage;
    public Image hourglassEmptyImage;
    private Coroutine currentTimerCoroutine = null;
    
    private void Start()
    {
        GameManager.Instance.onNewDayTimerStarted.AddListener(StartVisualTimer);
    }

    private void OnDisable()
    {
        GameManager.Instance.onNewDayTimerStarted.RemoveListener(StartVisualTimer);

    }
    
    private void StartVisualTimer()
    {
        if (currentTimerCoroutine != null)
        {
            StopCoroutine(currentTimerCoroutine);
        }
        
        currentTimerCoroutine = StartCoroutine(VisualTimerCoroutine());
    }
    
    private System.Collections.IEnumerator VisualTimerCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < GameManager.Instance.workDayTime)
        {
            elapsedTime += Time.deltaTime;
            hourglassEmptyImage.fillAmount = 1 - (elapsedTime / GameManager.Instance.workDayTime);
            hourglassFillImage.fillAmount = 0 + (elapsedTime / GameManager.Instance.workDayTime);
            yield return null;
        }
        hourglassFillImage.fillAmount = 1;
        hourglassEmptyImage.fillAmount = 0;
    }
}

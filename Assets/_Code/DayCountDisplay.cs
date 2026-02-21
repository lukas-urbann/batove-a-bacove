using Controllers;
using TMPro;
using UnityEngine;

public class DayCountDisplay : MonoBehaviour
{
    [SerializeField] private string prefix = "Dny v moci";
    [SerializeField] private TMP_Text _dayCountText;

    private void Start()
    {
        DayManager.Instance.OnNewDayStarted += UpdateText;
    }

    private void OnDisable()
    {
        DayManager.Instance.OnNewDayStarted -= UpdateText;
    }

    private void UpdateText(int day)
    {
        _dayCountText.text = $"{prefix} {day}";
    }
}

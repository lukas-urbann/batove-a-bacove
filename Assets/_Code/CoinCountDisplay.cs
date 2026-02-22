using Controllers;
using TMPro;
using UnityEngine;

public class CoinCountDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _dayCountText;
    
    private void Start()
    {
        GameManager.Instance.onCoinsUpdated.AddListener(UpdateText);
    }

    private void OnDisable()
    {
        GameManager.Instance.onCoinsUpdated.RemoveListener(UpdateText);
    }

    private void UpdateText(int coins)
    {
        _dayCountText.text = $"{coins} grošů";
    }
}

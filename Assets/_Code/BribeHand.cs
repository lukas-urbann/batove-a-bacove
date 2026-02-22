using System;
using Controllers;
using UnityEngine;
using UnityEngine.UI;

public class BribeHand : MonoBehaviour
{
    [SerializeField] private Animator handAnimator;
    [SerializeField] private Button handButton;
    
    private void Start()
    {
        GameManager.Instance.onJudgedDismissed.AddListener(Hide);
        GameManager.Instance.onJudgedBribery.AddListener(Show);
    }

    private void OnDisable()
    {
        GameManager.Instance.onJudgedDismissed.RemoveListener(Hide);
        GameManager.Instance.onJudgedBribery.RemoveListener(Show);
    }

    private void Show()
    {
        handAnimator.Play("HandSlideIn");
        handButton.interactable = true;
    }

    private void Hide()
    {
        handAnimator.Play("HandHidden");
        handButton.interactable = false;
    }

    public void AcceptBribe()
    {
        GameManager.Instance.JudgeResponse(false);
        AudioManager.Instance.PlayBribeTake();
        int bribeAmount = UnityEngine.Random.Range(5, 16);
        GameManager.Instance.AddCoins(bribeAmount);
        GameManager.Instance.BriberyBias();
    }
}

using Controllers;
using UnityEngine;
using UnityEngine.Events;

public class HappinessReaction : MonoBehaviour
{
    [Header("The Rich")]
    public UnityEvent richVeryHappy;
    public UnityEvent richHappy;
    public UnityEvent richMild;
    public UnityEvent richAngry;
    public UnityEvent richVeryAngry;
    
    [Header("The Poor")]
    public UnityEvent poorVeryHappy;
    public UnityEvent poorHappy;
    public UnityEvent poorMild;
    public UnityEvent poorAngry;
    public UnityEvent poorVeryAngry;
    
    private void Start()
    {
        GameManager.Instance.onRichHappinessUpdated.AddListener(EvaluateReactionRich);
        GameManager.Instance.onPoorHappinessUpdated.AddListener(EvaluateReactionPoor);
    }

    private void OnDisable()
    {
        GameManager.Instance.onRichHappinessUpdated.RemoveListener(EvaluateReactionRich);
        GameManager.Instance.onPoorHappinessUpdated.RemoveListener(EvaluateReactionPoor);
    }
    
    private void EvaluateReactionRich(float val)
    {
         if (val >= 0.8f)
         {
             richVeryHappy.Invoke();
         }
         else if (val >= 0.6f)
         {
             richHappy.Invoke();
         }
         else if (val >= 0.4f)
         {
             richMild.Invoke();
         }
         else if (val >= 0.2f)
         {
             richAngry.Invoke();
         }
         else
         {
             richVeryAngry.Invoke();
         }
    }
    
    private void EvaluateReactionPoor(float val)
    {
        if (val >= 0.8f)
        {
            poorVeryHappy.Invoke();
        }
        else if (val >= 0.6f)
        {
            poorHappy.Invoke();
        }
        else if (val >= 0.4f)
        {
            poorMild.Invoke();
        }
        else if (val >= 0.2f)
        {
            poorAngry.Invoke();
        }
        else
        {
            poorVeryAngry.Invoke();
        }
    }
}

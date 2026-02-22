using UnityEngine;
using UnityEngine.Events;

public class MenuDeathCall : MonoBehaviour
{
    public UnityEvent onDeath;
    
    public void TriggerEvent()
    {
        onDeath?.Invoke();
    }
}

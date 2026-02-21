using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TypeEffect typeEffect;

    void Start()
    {
        typeEffect.StartTyping("Test scrolling text.");
    }
}
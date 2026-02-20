using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public TypeEffect typeEffect;

    void Start()
    {
        typeEffect.StartTyping("Your honor, the imposter is sus.");
    }
}
using UnityEngine;
using TMPro;
using System.Collections;

public class TypeEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float typingSpeed = 0.03f;
    private Coroutine typingCoroutine;
    private string fullText;
    private bool isTyping;

    public void StartTyping(string text)
    {
        fullText = text;
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = fullText;
        textComponent.maxVisibleCharacters = 0;
        textComponent.ForceMeshUpdate();
        int totalVisibleCharacters = textComponent.textInfo.characterCount;
        for (int i = 0; i <= totalVisibleCharacters; i++)
        {
            textComponent.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    
}
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class InkwellInteractable : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private QuillInteractable quill;

    public void OnPointerClick(PointerEventData eventData)
    {
        quill.ReturnToInkwell();
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class InteractableBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }

    protected virtual void OnClick()
    {
        Debug.Log($"Clicked: {gameObject.name}");
    }

    protected virtual void OnHoverEnter()
    {
        Debug.Log($"Hover Enter: {gameObject.name}");
    }

    protected virtual void OnHoverExit()
    {
        Debug.Log($"Hover Exit: {gameObject.name}");
    }
}
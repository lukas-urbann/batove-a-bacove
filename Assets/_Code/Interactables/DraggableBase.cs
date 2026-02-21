using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
public class DraggableBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _offset;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - GetMouseWorldPos(eventData);
        OnDragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = GetMouseWorldPos(eventData) + _offset;
        OnDragging();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }

    protected virtual void OnDragStart()
    {
        Debug.Log($"Drag Start: {gameObject.name}");
    }

    protected virtual void OnDragging()
    {
        Debug.Log($"Drag: {gameObject.name}");
    }

    protected virtual void OnDragEnd()
    {
        Debug.Log($"Drag End: {gameObject.name}");
    }

    protected virtual void OnHoverEnter()
    {
        Debug.Log($"Hover Enter: {gameObject.name}");
    }

    protected virtual void OnHoverExit()
    {
        Debug.Log($"Hover Exit: {gameObject.name}");
    }

    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
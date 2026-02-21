using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class DraggableBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 _offset;
    
    [SerializeField] protected float returnSpeed = 4f;
    [SerializeField] protected float hoverDarken = 0.6f;
    
    protected Vector3 InitialPosition { get; private set; }
    protected Quaternion InitialRotation { get; private set; }
    protected SpriteRenderer Renderer { get; private set; }
    protected Color OriginalColor { get; private set; }
    protected int OriginalSortingOrder { get; private set; }
    protected bool IsDragging { get; set; }
    
    public static bool AnyDragging { get; private set; }
    
    protected virtual void Awake()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
        Renderer = GetComponent<SpriteRenderer>();
        OriginalColor = Renderer.color;
        OriginalSortingOrder = Renderer.sortingOrder;
    }
    
    protected virtual void Update()
    {
        if (!IsDragging)
        {
            transform.position = Vector3.Lerp(transform.position, InitialPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, InitialRotation, Time.deltaTime * returnSpeed);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _offset = transform.position - GetMouseWorldPos(eventData);
        OnDragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanDrag()) return;
        transform.position = GetMouseWorldPos(eventData) + _offset;
        OnDragging();
    }
    
    protected virtual bool CanDrag()
    {
        return true;
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
        DraggableBase.AnyDragging = true;
        IsDragging = true;
        Renderer.color = OriginalColor;
        Renderer.sortingOrder = OriginalSortingOrder + 1;
    }

    protected virtual void OnDragging()
    {
        Debug.Log($"Drag: {gameObject.name}");
    }

    protected virtual void OnDragEnd()
    {
        DraggableBase.AnyDragging = false;
        IsDragging = false;
        Renderer.color = OriginalColor;
        Renderer.sortingOrder = OriginalSortingOrder;
        Debug.Log($"Drag End: {gameObject.name}");
    }

    protected virtual void OnHoverEnter()
    {
        if (DraggableBase.AnyDragging) return;
        Renderer.color = OriginalColor * hoverDarken;
        Debug.Log($"Hover Enter: {gameObject.name}");
    }

    protected virtual void OnHoverExit()
    {
        if (DraggableBase.AnyDragging) return;
        Renderer.color = OriginalColor;
        Debug.Log($"Hover Exit: {gameObject.name}");
    }

    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
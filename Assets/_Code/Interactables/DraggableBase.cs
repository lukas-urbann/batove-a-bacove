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
    
    private Vector3 InitialPosition { get; set; }
    private Quaternion InitialRotation { get;  set; }
    protected SpriteRenderer Renderer { get; private set; }
    protected Color OriginalColor { get; private set; }
    private int OriginalSortingOrder { get; set; }
    protected bool IsDragging { get; set; }
    
    protected static bool AnyDragging { get; private set; }
    
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
        GameState.Instance.isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //if (!CanDrag()) return;
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
        GameState.Instance.isDragging = false;
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
        AnyDragging = true;
        IsDragging = true;
        Renderer.color = OriginalColor;
        Renderer.sortingOrder = OriginalSortingOrder + 2;
        Cursor.visible = false;
    }

    protected virtual void OnDragging()
    {
        
    }

    protected virtual void OnDragEnd()
    {
        AnyDragging = false;
        IsDragging = false;
        Renderer.color = OriginalColor;
        Renderer.sortingOrder = OriginalSortingOrder;
        Cursor.visible = true;
    }

    protected virtual void OnHoverEnter()
    {
        if (AnyDragging) return;
        Renderer.color = OriginalColor * hoverDarken;
    }

    protected virtual void OnHoverExit()
    {
        if (AnyDragging) return;
        Renderer.color = OriginalColor;
    }

    private Vector3 GetMouseWorldPos(PointerEventData eventData)
    {
        Vector3 pos = eventData.position;
        pos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(pos);
    }
}
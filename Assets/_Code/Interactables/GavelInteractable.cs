using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GavelInteractable : DraggableBase
{
    [SerializeField] private float returnSpeed = 8f;
    [SerializeField] private float dragAngle = -45f;
    [SerializeField] private float hoverDarken = 0.6f;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private SpriteRenderer _renderer;
    private Color _originalColor;
    private bool _isDragging;
    
    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _renderer = GetComponent<SpriteRenderer>();
        _originalColor = _renderer.color;
    }

    private void Update()
    {
        if (!_isDragging)
        {
            transform.position = Vector3.Lerp(transform.position, _initialPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _initialRotation, Time.deltaTime * returnSpeed);
        }
    }

    protected override void OnDragStart()
    {
        _isDragging = true;
        transform.rotation = Quaternion.Euler(0, 0, dragAngle);
    }

    protected override void OnDragEnd()
    {
        _isDragging = false;
    }

    protected override void OnHoverEnter()
    {
        _renderer.color = _originalColor * hoverDarken;
    }

    protected override void OnHoverExit()
    {
        _renderer.color = _originalColor;
    }
}
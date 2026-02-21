using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class GavelInteractable : DraggableBase
{
    [SerializeField] private float returnSpeed = 4f;
    [SerializeField] private float dragAngle = -45f;
    [SerializeField] private float hoverDarken = 0.6f;
    [SerializeField] private float smashThreshold = 100f;
    [SerializeField] private float smashMinDistance = 0.7f;
    [SerializeField] private float smashAngle = 50f;
    
    [SerializeField] private float shakeDuration = 0.4f;
    [SerializeField] private float shakeMagnitude = 0.3f;
    [SerializeField] private Camera mainCamera;

    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private SpriteRenderer _renderer;
    private Color _originalColor;
    private bool _isDragging;
    private bool _isSmashed;
    private float _previousY;
    private float _dragStartY;

    private void Awake()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _renderer = GetComponent<SpriteRenderer>();
        _originalColor = _renderer.color;
    }

    private void Update()
    {
        if (_isDragging)
        {
            float velocity = (_previousY - transform.position.y) / Time.deltaTime;
            float distance = _dragStartY - transform.position.y;
            _previousY = transform.position.y;

            if (!_isSmashed && velocity > smashThreshold && distance > smashMinDistance)
            {
                _isSmashed = true;
                transform.rotation = Quaternion.Euler(0, 0, smashAngle);
                OnSmash();
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _initialPosition, Time.deltaTime * returnSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, _initialRotation, Time.deltaTime * returnSpeed);
        }
    }

    protected override void OnDragStart()
    {
        _isDragging = true;
        _isSmashed = false;
        _previousY = transform.position.y;
        _dragStartY = transform.position.y;
        transform.rotation = Quaternion.Euler(0, 0, dragAngle);
    }

    protected override void OnDragEnd()
    {
        _isDragging = false;
        _isSmashed = false;
    }

    protected override void OnHoverEnter()
    {
        _renderer.color = _originalColor * hoverDarken;
    }

    protected override void OnHoverExit()
    {
        _renderer.color = _originalColor;
    }

    protected virtual void OnSmash()
    {
        Debug.Log("Gavel smashed");
        StartCoroutine(ShakeCamera());
        StartCoroutine(SmashDelay());
    }
    
    private IEnumerator ShakeCamera()
    {
        Vector3 originalPos = mainCamera.transform.position;
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            mainCamera.transform.position = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        mainCamera.transform.position = originalPos;
    }

    private IEnumerator SmashDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _isDragging = false;
        _isSmashed = false;
    }
    
    protected override bool CanDrag()
    {
        return !_isSmashed;
    }
}
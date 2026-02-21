using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class QuillInteractable : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private VoteZone leftZone;
    [SerializeField] private VoteZone rightZone;
    [SerializeField] private GameObject inkPrefab;
    [SerializeField] private float minPointDistance = 0.05f;
    [SerializeField] private Transform quillTip;
    [SerializeField] private PaperInteractable paper;
    [SerializeField] private float hoverDarken = 0.6f;
    [SerializeField] private Vector3 inkwellOffset;

    private SpriteRenderer _renderer;
    private Color _originalColor;
    private Vector3 _initialPosition;
    private bool _isPickedUp;
    private bool _isDrawing;

    private LineRenderer _currentLine;
    private List<Vector3> _currentPoints = new List<Vector3>();
    private VoteZone _activeZone;
    
    public static bool IsPickedUp { get; private set; }

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _originalColor = _renderer.color;
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (_isPickedUp)
        {
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = transform.position.z;
            transform.position = mouseWorld;

            if (Mouse.current.leftButton.isPressed)
            {
                VoteZone zone = GetCurrentZone();
                if (zone != null)
                {
                    if (!_isDrawing || _activeZone != zone)
                        StartNewLine(zone);
                    _isDrawing = true;
                    AddPoint();
                }
                else
                {
                    StopDrawing();
                }
            }
            else
            {
                StopDrawing();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isPickedUp)
            PickUp();
    }
    

    private IEnumerator ReturnLerp()
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;
        float duration = 0.5f;
        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, _initialPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = _initialPosition;
    }

    

    private void PickUp()
    {
        _isPickedUp = true;
        IsPickedUp = true;
    }

    public void ReturnToInkwell()
    {
        _isPickedUp = false;
        IsPickedUp = false;
        StopDrawing();
        StartCoroutine(ReturnLerp());
    }
    private void StopDrawing()
    {
        if (!_isDrawing) return;
        _isDrawing = false;
        _activeZone = null;
        _currentLine = null;
        _currentPoints.Clear();
    }

    private VoteZone GetCurrentZone()
    {
        if (leftZone.Contains(quillTip.position)) return leftZone;
        if (rightZone.Contains(quillTip.position)) return rightZone;
        return null;
    }

    private void StartNewLine(VoteZone zone)
    {
        _activeZone = zone;
        _currentPoints.Clear();
        GameObject inkObj = Instantiate(inkPrefab);
        _currentLine = inkObj.GetComponent<LineRenderer>();
        _currentLine.useWorldSpace = true;
        zone.AddInkLine(inkObj);
    }

    private void AddPoint()
    {
        Vector3 tipPos = quillTip.position;
        if (_currentPoints.Count > 0 && Vector3.Distance(_currentPoints[^1], tipPos) < minPointDistance)
            return;
        _currentPoints.Add(tipPos);
        _currentLine.positionCount = _currentPoints.Count;
        _currentLine.SetPositions(_currentPoints.ToArray());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isPickedUp) return;
        _renderer.color = _originalColor * hoverDarken;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isPickedUp) return;
        _renderer.color = _originalColor;
    }
}
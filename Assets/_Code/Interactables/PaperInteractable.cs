using UnityEngine;
using System.Collections;

public class PaperInteractable : DraggableBase
{
    [SerializeField] private VoteZone leftZone;
    [SerializeField] private VoteZone rightZone;
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float offScreenY = -20f;
    [SerializeField] private float returnDelay = 0.1f;
    [SerializeField] private float swipeDownThreshold = 1f;

    private Vector3 _initialPosition;
    private bool _isAnimating;

    protected override void Awake()
    {
        base.Awake();
        _initialPosition = transform.position;
    }

    protected override void Update()
    {
        
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        float draggedDown = _initialPosition.y - transform.position.y;
        if (draggedDown > swipeDownThreshold && !_isAnimating)
            StartCoroutine(SlideOffAndBack());
        else if (!_isAnimating)
            transform.position = _initialPosition;
    }

    public void OnDecisionFinalized()
    {
        if (!_isAnimating)
            StartCoroutine(SlideOffAndBack());
    }

    private IEnumerator SlideOffAndBack()
    {
        _isAnimating = true;
        transform.position = _initialPosition;

        Vector3 offScreen = new Vector3(_initialPosition.x, offScreenY, _initialPosition.z);

        while (Vector3.Distance(transform.position, offScreen) > 0.01f)
        {
            Vector3 prev = transform.position;
            transform.position = Vector3.Lerp(transform.position, offScreen, Time.deltaTime * slideSpeed);
            Vector3 delta = transform.position - prev;  
            leftZone.ShiftInk(delta);
            rightZone.ShiftInk(delta);
            yield return null;
        }

        leftZone.ClearInk();
        rightZone.ClearInk();

        yield return new WaitForSeconds(returnDelay);

        while (Vector3.Distance(transform.position, _initialPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, _initialPosition, Time.deltaTime * slideSpeed);
            yield return null;
        }

        transform.position = _initialPosition;
        _isAnimating = false;
    }
    
    protected override bool CanDrag()
    {
        return !QuillInteractable.IsPickedUp && !_isAnimating;
    }
}
using UnityEngine;
using System.Collections;

public class PaperInteractable : InteractableBase
{
    [SerializeField] private VoteZone leftZone;
    [SerializeField] private VoteZone rightZone;
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private float offScreenY = -20f;
    [SerializeField] private float returnDelay = 0.1f;

    private Vector3 _initialPosition;
    private Coroutine _slideCoroutine;

    protected void Awake()
    {
        _initialPosition = transform.position;
    }

    protected override void OnClick()
    {
        /*
        if (_slideCoroutine == null)
            _slideCoroutine = StartCoroutine(SlideOffAndBack());
            */

    }
    
    public void AlternativeClick()
    {
        if (_slideCoroutine == null)
            _slideCoroutine = StartCoroutine(SlideOffAndBack());
    }

    public void OnDecisionFinalized()
    {
        if (_slideCoroutine == null)
            _slideCoroutine = StartCoroutine(SlideOffAndBack());
        
    }

    private IEnumerator SlideOffAndBack()
    {
        Vector3 offScreen = new Vector3(_initialPosition.x, offScreenY, _initialPosition.z);
    
        while (Vector3.Distance(transform.position, offScreen) > 0.01f)
        {
            AudioManager.Instance.PlayPaperSlide();
            Vector3 prev = transform.position;
            transform.position = Vector3.Lerp(transform.position, offScreen, Time.deltaTime * slideSpeed);
            Vector3 delta = transform.position - prev;
            leftZone.ShiftInk(delta);
            rightZone.ShiftInk(delta);
            yield return null;
        }

        leftZone.ClearInk();
        rightZone.ClearInk();
        
        leftZone.gameObject.SetActive(true);
        rightZone.gameObject.SetActive(true);

        yield return new WaitForSeconds(returnDelay);

        while (Vector3.Distance(transform.position, _initialPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, _initialPosition, Time.deltaTime * slideSpeed);
            yield return null;
        }

        transform.position = _initialPosition;
        _slideCoroutine = null;
    }
}
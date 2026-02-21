using System.Collections;
using UnityEngine;

public class GavelInteractable : DraggableBase
{
    [SerializeField] private float dragAngle = -45f;
    [SerializeField] private float smashThreshold = 100f;
    [SerializeField] private float smashMinDistance = 0.7f;
    [SerializeField] private float smashAngle = 50f;
    
    [SerializeField] private float shakeDuration = 0.4f;
    [SerializeField] private float shakeMagnitude = 0.3f;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider2D validZone;

    private bool _isSmashed;
    private float _previousY;
    private float _dragStartY;

    private void Update()
    {
        base.Update();
        
        if (IsDragging)
        {
            float velocity = (_previousY - transform.position.y) / Time.deltaTime;
            float distance = _dragStartY - transform.position.y;
            _previousY = transform.position.y;

            if (!_isSmashed && velocity > smashThreshold && distance > smashMinDistance && IsInValidZone())
            {
                _isSmashed = true;
                transform.rotation = Quaternion.Euler(0, 0, smashAngle);
                OnSmash();
            }
        }
    }
    
    private bool IsInValidZone()
    {
        return validZone.OverlapPoint(transform.position);
    }

    protected override void OnDragStart()
    {
        base.OnDragStart();
        _isSmashed = false;
        _previousY = transform.position.y;
        _dragStartY = transform.position.y;
        transform.rotation = Quaternion.Euler(0, 0, dragAngle);
    }

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        _isSmashed = false;
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
        IsDragging = false;
        _isSmashed = false;
    }
    
    protected override bool CanDrag()
    {
        return !_isSmashed;
    }
}
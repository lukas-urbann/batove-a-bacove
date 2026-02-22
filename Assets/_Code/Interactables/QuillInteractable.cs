using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class QuillInteractable : DraggableBase
{
    [SerializeField] private VoteZone leftZone;
    [SerializeField] private VoteZone rightZone;
    [SerializeField] private GameObject inkPrefab;
    [SerializeField] private float minPointDistance = 0.05f;
    [SerializeField] private Transform quillTip;
    [SerializeField] private PaperInteractable paper;

    private LineRenderer _currentLine;
    private List<Vector3> _currentPoints = new List<Vector3>();
    private VoteZone _activeZone;


    public override void OnDragAudio()
    {
        if (_activeZone != null)
        {
            AudioManager.Instance.PlayQuillDraw();
        }
        
    }
    
    protected override void OnDragStart()
    {
        base.OnDragStart();
        AudioManager.Instance.PlayQuillPickup();
    }

    protected override void OnDragging()
    {
        
        VoteZone zone = GetCurrentZone();

        if (zone != null)
        {
            if (_activeZone != zone)
                StartNewLine(zone);

            AddPoint(transform.position);
        }
        else if (_activeZone != null)
        {
            FinishLine();
        }
    }

    private VoteZone GetCurrentZone()
    {
        if (leftZone.Contains(quillTip.position))  return leftZone;
        if (rightZone.Contains(quillTip.position)) return rightZone;
        return null;
    }

    private void AddPoint(Vector3 pos)
    {
        Vector3 tipPos = quillTip.position;
        if (_currentPoints.Count > 0 && Vector3.Distance(_currentPoints[^1], tipPos) < minPointDistance)
            return;
        _currentPoints.Add(tipPos);
        _currentLine.positionCount = _currentPoints.Count;
        _currentLine.SetPositions(_currentPoints.ToArray());
        
    }
    
    private Coroutine _drawLoopCoroutine;

    private void StartNewLine(VoteZone zone)
    {
        _activeZone = zone;
        _currentPoints.Clear();
        GameObject inkObj = Instantiate(inkPrefab, paper.transform, true);
        _currentLine = inkObj.GetComponent<LineRenderer>();
        _currentLine.useWorldSpace = true;
        zone.AddInkLine(inkObj);

        if (_drawLoopCoroutine != null) StopCoroutine(_drawLoopCoroutine);
    }

    private void FinishLine()
    {
        if (_drawLoopCoroutine != null)
        {
            StopCoroutine(_drawLoopCoroutine);
            _drawLoopCoroutine = null;
        }
        _activeZone = null;
        _currentLine = null;
    }
    

    protected override void OnDragEnd()
    {
        base.OnDragEnd();
        if (_activeZone != null) FinishLine();
    }
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class VoteZone : MonoBehaviour
{
    [SerializeField] private bool isLeft;

    private Collider2D _collider;
    private List<GameObject> _inkLines = new List<GameObject>();
    private bool _hasInk;

    [SerializeField] private GameObject otherVoteZone;

    public UnityEvent onSelect = new();
    public UnityEvent onReset = new();

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public bool Contains(Vector3 point)
    {
        return _collider.OverlapPoint(point);
    }

    public void AddInkLine(GameObject line)
    {
        _inkLines.Add(line);
        if (!_hasInk)
        {
            _hasInk = true;
            GameState.Instance.SetZoneInked(isLeft, true);
            
            otherVoteZone.gameObject.SetActive(false);
            onSelect?.Invoke();
        }
    }

    public void ClearInk()
    {
        foreach (var line in _inkLines)
            Destroy(line);
        _inkLines.Clear();
        _hasInk = false;
        GameState.Instance.SetZoneInked(isLeft, false);
        onReset?.Invoke();
    }
    
    public void ShiftInk(Vector3 delta)
    {
        foreach (var line in _inkLines)
        {
            LineRenderer lr = line.GetComponent<LineRenderer>();
            Vector3[] points = new Vector3[lr.positionCount];
            lr.GetPositions(points);
            for (int i = 0; i < points.Length; i++)
                points[i] += delta;
            lr.SetPositions(points);
        }
    }
}
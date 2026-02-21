using UnityEngine;

public class VoteZone : MonoBehaviour
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public bool Contains(Vector3 point)
    {
        return _collider.OverlapPoint(point);
    }
}
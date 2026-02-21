using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }

    public enum Decision { None, Left, Right, Conflict }

    public Decision CurrentDecision { get; private set; }

    private bool _leftHasInk;
    private bool _rightHasInk;

    private void Awake()
    {
        Instance = this;
    }

    public void SetZoneInked(bool isLeft, bool hasInk)
    {
        if (isLeft) _leftHasInk = hasInk;
        else _rightHasInk = hasInk;
        if (_leftHasInk && _rightHasInk)
            CurrentDecision = Decision.Conflict;
        else if (_leftHasInk)
            CurrentDecision = Decision.Left;
        else if (_rightHasInk)
            CurrentDecision = Decision.Right;
        else
            CurrentDecision = Decision.None;
    }

    public bool CanGavel()
    {
        return CurrentDecision == Decision.Left || CurrentDecision == Decision.Right;
    }

    public void ClearAll()
    {
        _leftHasInk = false;
        _rightHasInk = false;
        CurrentDecision = Decision.None;
    }

    public void FinalizeDecision()
    {
        ClearAll();
    }
}
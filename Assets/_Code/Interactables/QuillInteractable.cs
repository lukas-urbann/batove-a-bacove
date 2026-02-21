using UnityEngine;

public class QuillInteractable : DraggableBase
{
    protected virtual void OnSign(Vector3 position)
    {
        Debug.Log($"Signed at: {position}");
    }
}
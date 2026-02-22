using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CameraScrollController : MonoBehaviour
{
    public float panSpeed = 30f;
    public float edgeThreshold = 25f;
    public float leftLimit = -28f;
    public float rightLimit = 28f;

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        float mouseX = Mouse.current.position.ReadValue().x;
        float screenWidth = Screen.width;
        Vector3 pos = transform.position;

        float direction = 0f;

        if (mouseX <= edgeThreshold)
            direction = -1f;
        else if (mouseX >= screenWidth - edgeThreshold)
            direction = 1f;

        if ((direction < 0 && pos.x > leftLimit) || (direction > 0 && pos.x < rightLimit))
            pos.x += direction * panSpeed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        transform.position = pos;
    }
}
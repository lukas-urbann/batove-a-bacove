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

        Vector3 pos = transform.position;
        float mouseX = Mouse.current.position.ReadValue().x;
        float screenWidth = Screen.width;
        
        if (mouseX <= edgeThreshold) 
            pos.x -= panSpeed * Time.deltaTime;
        if (mouseX >= screenWidth - edgeThreshold) 
            pos.x += panSpeed * Time.deltaTime;
        
        pos.x = Mathf.Clamp(pos.x, leftLimit, rightLimit);
        transform.position = pos;
    }
}
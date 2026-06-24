using UnityEngine;

public class TestTouch : MonoBehaviour
{
    public InputManager inputManager;
    private Camera cameraMain;

    private float lockedZ;

    private void Awake()
    {
        cameraMain = Camera.main;
    }

    private void OnEnable()
    {
        inputManager.onDrag += Move;
        inputManager.onStartTouch += StartDrag;
    }

    private void OnDisable()
    {
        inputManager.onDrag -= Move;
        inputManager.onStartTouch -= StartDrag;
    }

    private void StartDrag(Vector2 screenPosition, float time)
    {
        // Lock the object's original Z so it NEVER changes depth
        lockedZ = transform.position.z;
    }

    public void Move(Vector2 screenPosition)
    {
        Ray ray = cameraMain.ScreenPointToRay(screenPosition);

        // LOCK movement to a fixed Y height (ground plane)
        float groundY = transform.position.y;

        if (ray.direction.y == 0) return;

        float t = (groundY - ray.origin.y) / ray.direction.y;

        Vector3 worldPosition = ray.origin + ray.direction * t;

        // HARD LOCK Y so it NEVER moves up/down
        worldPosition.y = groundY;

        transform.position = worldPosition;
    }
}
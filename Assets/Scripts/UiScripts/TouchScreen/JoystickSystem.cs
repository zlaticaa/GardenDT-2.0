using UnityEngine;
using Terresquall;

public class JoystickSystem : MonoBehaviour
{
    public VirtualJoystick joystick;
    public float moveSpeed = 5f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (joystick == null || cam == null) return;

        // ✅ Correct input method for this asset
        float h = joystick.GetAxis("Horizontal");
        float v = joystick.GetAxis("Vertical");

        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = forward * v + right * h;

        cam.transform.position += move * moveSpeed * Time.deltaTime;
    }
}

using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody; // 引用玩家的 Transform
    public Transform cameraTransform; // 引用相机的 Transform

    float xRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        // 旋转相机的上下视角
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 旋转玩家的水平视角
        playerBody.Rotate(Vector3.up * mouseX);
    }
}

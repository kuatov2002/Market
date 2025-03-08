using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")] [SerializeField]
        private float moveSpeed = 5f;

        [SerializeField] private float lookSensitivity = 2f;

        [Header("References")] [SerializeField]
        private Transform cameraTransform;

        [SerializeField] private FixedJoystick moveJoystick;

        private float rotationX = 0f;

        private void Update()
        {
            // Handle movement using joystick
            if (moveJoystick != null)
            {
                float horizontalInput = moveJoystick.Horizontal;
                float verticalInput = moveJoystick.Vertical;

                Vector3 move = transform.right * horizontalInput + transform.forward * verticalInput;
                transform.position += move * moveSpeed * Time.deltaTime;
            }

            // Handle looking around
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Check if touch is on the right side of the screen (not on joystick)
                if (touch.position.x > Screen.width * 0.5f)
                {
                    // Look around functionality
                    if (touch.phase == TouchPhase.Moved)
                    {
                        float xRotation = touch.deltaPosition.y * lookSensitivity * -1;
                        float yRotation = touch.deltaPosition.x * lookSensitivity;

                        // Vertical rotation (pitch)
                        rotationX += xRotation;
                        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
                        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

                        // Horizontal rotation (yaw)
                        transform.Rotate(Vector3.up * yRotation);
                    }
                }
            }
        }
    }
}
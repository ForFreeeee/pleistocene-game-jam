using UnityEngine;
using UnityEngine.InputSystem;

public class MoveManager : MonoBehaviour
{
    [SerializeField]
    Camera cameraRotate;

    [SerializeField]
    GameObject focusGO;

    private Vector2 startingMousePosition;

    [SerializeField]
    float speed;

    bool isRotating = false;

    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            isRotating = true;
            startingMousePosition = Mouse.current.position.ReadValue();
        }
        else if (Mouse.current.rightButton.wasReleasedThisFrame)
        { 
            isRotating = false; 
        }

        if (isRotating)
        {
            Vector2 currentMousePosition = Mouse.current.position.ReadValue();
            Vector2 mouseMouvement = currentMousePosition - startingMousePosition;

            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                                cameraRotate.transform.up,
                                                mouseMouvement.x * speed);

            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                            cameraRotate.transform.right,
                                            -mouseMouvement.y * speed);
        }
    }
}

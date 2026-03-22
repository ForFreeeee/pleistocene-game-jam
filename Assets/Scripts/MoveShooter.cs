using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveShooter : MonoBehaviour
{

    [SerializeField]
    Camera cameraRotate;

    [SerializeField]
    GameObject focusGO;

    [SerializeField]
    float speed;

    [SerializeField]
    float m_setDelayValue;
    float m_currentDelay;

    void Update()
    {
        m_currentDelay -= Time.deltaTime;

    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (m_currentDelay < 0)
        {
            Vector2 val = Mouse.current.position.ReadValue();
            Debug.Log("Clicked detected Move");
            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                                cameraRotate.transform.up,
                                                -val.x * speed);

            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                            cameraRotate.transform.right,
                                            -val.y * speed);
        }
            

        m_currentDelay = m_setDelayValue;
    }
}

using System.Collections;
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
    float delay;

    bool isStart = false;

    private void Update()
    {
        if (Mouse.current.middleButton.IsPressed())
        {
            Vector2 val = Mouse.current.position.ReadValue();
            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                                cameraRotate.transform.up,
                                                -val.x * speed);

            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                            cameraRotate.transform.right,
                                            -val.y * speed);
        }
        

        //TO update better if we have time
        /*if (Mouse.current.middleButton.IsPressed() && !isStart)
        {
            isStart = true;
            StartCoroutine(MoveArround());
        }
        else if ( isStart)
        {
            StopCoroutine(MoveArround());
            isStart = false;

        }*/
    }

    IEnumerator MoveArround()
    {
        while (isStart)
        {
            Vector2 val = Mouse.current.position.ReadValue();
            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                                cameraRotate.transform.up,
                                                -val.x * speed);

            cameraRotate.transform.RotateAround(focusGO.transform.position,
                                            cameraRotate.transform.right,
                                            -val.y * speed);

            yield return new WaitForSeconds(delay);
        }
    }
}

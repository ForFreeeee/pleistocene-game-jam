using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class MossShooter : MonoBehaviour
{

    [SerializeField]
    GameObject m_moss;

    [SerializeField]
    float m_mossRadius;

    [SerializeField]
    int m_mossDensity;

    [SerializeField]
    Vector3 m_mossScale;

    List<GameObject> m_childMoss;

    [SerializeField]
    Camera m_camera;

    PlayerInput playerInput;

    [SerializeField]
    float m_setDelayValue;

    float m_currentDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_childMoss = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        m_currentDelay -= Time.deltaTime;
        
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Debug.Log("Clicked detected");
        Vector2 val = Mouse.current.position.ReadValue();
        Debug.Log(val);

        if (m_currentDelay < 0)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(val.x, val.y, Camera.main.nearClipPlane));
            Debug.Log(point);
            Ray r = new Ray(Camera.main.transform.position, point - Camera.main.transform.position);
            RaycastHit hit;
            GameObject hit_object;
            Debug.DrawRay(r.origin, r.direction * 100, Color.red, 3.0f);
            if (Physics.Raycast(r, out hit))
            {
                Debug.Log("Got a hit");
                hit_object = hit.collider.gameObject;
                for (int i = 0; i < m_mossDensity; i++)
                    GenerateMoss(hit, hit_object);
            }
            m_currentDelay = m_setDelayValue;
        }
    }

    void GenerateMoss(RaycastHit hit, GameObject hit_object) {
        Vector3 hitpoint = hit.point;
        Vector3 normal = hit.normal;
        Debug.Log(hit.normal);
        GameObject childMoss = Instantiate(m_moss, hitpoint, Quaternion.identity);
        childMoss.transform.localScale = m_mossScale;
        //Debug.Log(childMoss.transform.up);
        childMoss.transform.rotation = Quaternion.FromToRotation(childMoss.transform.up, hit.normal); 
        //childMoss.transform.rotation.SetLookRotation(hit.normal);
        m_childMoss.Add(childMoss);    
    }
}

using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class MossShooter : MonoBehaviour
{

    [SerializeField]
    GameObject m_moss;

    [SerializeField]
    float m_radius;

    [SerializeField]
    int m_density;

    List<GameObject> m_childMoss;

    [SerializeField]
    Camera m_camera;

    PlayerInput playerInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_childMoss = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        Debug.Log("Clicked detected");
        // context.action

        Ray r = m_camera.ScreenPointToRay(m_camera.ScreenToViewportPoint(Mouse.current.position.ReadValue()));
        RaycastHit hit;
        GameObject hit_object;
        Debug.DrawRay(r.origin, r.direction * 100, Color.red, 3.0f);
        if (Physics.Raycast(r, out hit))
        {
            Debug.Log("Got a hit");
            hit_object = hit.collider.gameObject;
            for (int i = 0; i < m_density; i++) 
                GenerateMoss(hit, hit_object);  
        }
    }

    void GenerateMoss(RaycastHit hit, GameObject hit_object) {
            Vector3 hitpoint = hit.transform.position;
            Vector3 normal = hit.normal;
            GameObject childMoss = Instantiate(m_moss, hitpoint, hit.transform.rotation);
            m_childMoss.Add(childMoss);
            
    }
}

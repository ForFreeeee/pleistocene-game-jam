using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MossShooter : MonoBehaviour
{
    [SerializeField]
    GameObject m_parentMoss;
    [SerializeField]
    GameObject m_childMoss;

    [SerializeField]
    float m_mossRadius;

    [SerializeField]
    float m_mossRadiusShift;

    [SerializeField]
    float m_mossAngleVariation;

    [SerializeField]
    int m_mossDensity;

    [SerializeField]
    float m_mossAmountVariation;

    [SerializeField]
    Vector3 m_mossScale;

    List<GameObject> m_ParentMossObjects;

    [SerializeField]
    Camera m_camera;

    PlayerInput playerInput;

    [SerializeField]
    float m_setDelayValue;

    float m_currentDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_ParentMossObjects = new List<GameObject>();
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
            Debug.DrawRay(r.origin, r.direction * 100, Color.red, 3.0f);
            if (Physics.Raycast(r, out hit))
            {
                MossCoverable moss_coverable = hit.collider.gameObject.GetComponent<MossCoverable>();
                if (moss_coverable == null)
                    return;
                Debug.Log("Got a hit");
                GenerateMoss(hit);
            }
            m_currentDelay = m_setDelayValue;
        }
    }

<<<<<<< HEAD
    void GenerateMoss(RaycastHit hit, GameObject hit_object) {
       Vector3 hitpoint = hit.point;
=======
    void GenerateMoss(RaycastHit hit) {
        Vector3 hitpoint = hit.point;
>>>>>>> d720dbfa92a28ca5e90ca25e433b920e376b4689
        Vector3 normal = hit.normal;
        Debug.Log(hit.normal);
        GameObject parentMoss = Instantiate(m_parentMoss, hitpoint, Quaternion.identity);
        //GenerateRandomSubMoss(normal, childMoss);
        parentMoss.transform.localScale = m_mossScale;
        //Debug.Log(childMoss.transform.up);
<<<<<<< HEAD
        childMoss.transform.Translate(hit.normal * Random.Range(-0.03f, 0.0f));
        childMoss.transform.rotation = Quaternion.FromToRotation(childMoss.transform.up, hit.normal);
        childMoss.transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
        //childMoss.transform.rotation.SetLookRotation(hit.normal);
        m_childMoss.Add(childMoss);

        // Second part removing layer to make moss appear
        MossCoverable moss_coverable = hit.collider.gameObject.GetComponent<MossCoverable>();
            if (moss_coverable != null)    
                moss_coverable.AddMoss(hit.point, hit.textureCoord);
=======
        parentMoss.transform.rotation = Quaternion.FromToRotation(parentMoss.transform.up, hit.normal); 
        //childMoss.transform.rotation.SetLookRotation(hit.normal);
        m_ParentMossObjects.Add(parentMoss);    
        GenerateRandomSubMoss(normal, parentMoss);
    }

    void GenerateRandomSubMoss(Vector3 normal, GameObject parentMoss) {;
        float amount=Random.Range(m_mossDensity-m_mossAmountVariation, m_mossDensity+m_mossAmountVariation);
        Vector3 up=normal.normalized;
        Vector3 forwardHint = Vector3.forward;
        if (Vector3.Dot(up, forwardHint.normalized) > 0.999f)
        {
            forwardHint = Vector3.right; // fallback
        }
        Vector3 right=Vector3.Cross(normal, forwardHint).normalized;
        Vector3 forward=Vector3.Cross(normal, right).normalized;
        for (int i = 0; i < amount; i++)
        {
            float randomAngle = Random.Range(0, 360);
            float radius = Random.Range(m_mossRadius-m_mossRadiusShift, m_mossRadius+(m_mossRadiusShift/2));  
            Vector3 offset=radius * (Mathf.Cos(randomAngle) * right + Mathf.Sin(randomAngle) * forward);
            GameObject childMoss = Instantiate(m_childMoss, parentMoss.transform.position+offset, parentMoss.transform.rotation);
            
            float angleZ = Random.Range(parentMoss.transform.eulerAngles.z-m_mossAngleVariation, parentMoss.transform.eulerAngles.z + m_mossAngleVariation);
            float angleX = Random.Range(parentMoss.transform.eulerAngles.x-m_mossAngleVariation, parentMoss.transform.eulerAngles.x + m_mossAngleVariation);
            float angleY = Random.Range(0, 360);
            childMoss.transform.RotateAround(childMoss.transform.position, up, angleY);
            childMoss.transform.RotateAround(childMoss.transform.position, right, angleX);
            childMoss.transform.RotateAround(childMoss.transform.position, forward, angleZ);
            childMoss.transform.SetParent(parentMoss.transform, worldPositionStays: true);
        }  
>>>>>>> d720dbfa92a28ca5e90ca25e433b920e376b4689
    }
}

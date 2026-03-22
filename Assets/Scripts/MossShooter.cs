using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MossShooter : MonoBehaviour
{
    [SerializeField]
    GameObject sceneLight;

    [SerializeField]
    Vector3 m_mossScale;

    [SerializeField]
    MossData m_currentMoss;
    List<GameObject> m_ParentMossObjects;

    [SerializeField]
    Camera m_camera;

    PlayerInput playerInput;

    [SerializeField]
    float m_setDelayValue;

    float m_currentDelay;

    [SerializeField]
    GameObject[] m_toBeCovered;
    [SerializeField]
    float[] m_neededToWin;
    float[] m_coveredSurface;

    bool m_isSpraying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_ParentMossObjects = new List<GameObject>();
        m_isSpraying = false;

        m_coveredSurface = new float[m_toBeCovered.Length];
        for (int i = 0; i < m_coveredSurface.Length; i++)
            m_coveredSurface[i] = 0;
    }

    bool CheckWinCond()
    {
        for (int i = 0; i < m_coveredSurface.Length; i++)
        {
            if (m_coveredSurface[i] < m_neededToWin[i])
                return false;
        }
        return true;
    }

    void UpdateCoveredSurface(float coveredSurface, GameObject gameObject)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_currentDelay -= Time.deltaTime;
        Vector2 val = Mouse.current.position.ReadValue();
        //Debug.Log(val);

        if (m_currentDelay < 0 && m_isSpraying)
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
                if (CanPlaceParentMoss(hit.collider.gameObject.tag))
                {
                    GenerateMoss(hit, hit.collider.gameObject);
                }
            }
            m_currentDelay = m_setDelayValue;
        }
    }
    bool CanPlaceParentMoss(string hitTag)
    {
        if(hitTag=="OutOfBounds")
        {
            Debug.Log("Can't place moss here");
            return false;
        }
        return true;
    }

    
    public void OnShoot(InputAction.CallbackContext context)
    {
        //Debug.Log("Clicked detected");
        m_isSpraying = context.ReadValueAsButton();
    }
    
    void GenerateMoss(RaycastHit hit, GameObject hit_object) {        
        Vector3 hitpoint = hit.point;
        Vector3 normal = hit.normal;
        Debug.Log(hit.normal);
        GameObject parentMoss = Instantiate(m_currentMoss.parentMossPrefab, hitpoint, Quaternion.identity);
        MossController mossController=parentMoss.GetComponent<MossController>();
        mossController.hitPoint=hit.point;
        mossController.textureCoord=hit.textureCoord;
        mossController.moss_coverable=hit_object.GetComponent<MossCoverable>();
        mossController.directionalLight=sceneLight;
        //GenerateRandomSubMoss(normal, childMoss);
        parentMoss.transform.localScale = m_mossScale;
        parentMoss.GetComponent<MossController>().ShouldMossLive(hit_object.tag);
        //Debug.Log(childMoss.transform.up);
        parentMoss.transform.rotation = Quaternion.FromToRotation(parentMoss.transform.up, hit.normal); 
        //childMoss.transform.rotation.SetLookRotation(hit.normal);

        m_ParentMossObjects.Add(parentMoss);    
        GenerateRandomSubMoss(normal, parentMoss);
    }

    void GenerateRandomSubMoss(Vector3 normal, GameObject parentMoss) {;
        float amount=Random.Range(m_currentMoss.mossDensity-m_currentMoss.mossDensityVariation, m_currentMoss.mossDensity+m_currentMoss.mossDensityVariation);
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
            float radius = Random.Range(m_currentMoss.mossRadius-m_currentMoss.mossRadiusShift, m_currentMoss.mossRadius+(m_currentMoss.mossRadiusShift/2));  
            Vector3 offset=radius * (Mathf.Cos(randomAngle) * right + Mathf.Sin(randomAngle) * forward);
            GameObject childMoss = Instantiate(m_currentMoss.childMossPrefab, parentMoss.transform.position+offset, parentMoss.transform.rotation);
            float angleZ = Random.Range(-m_currentMoss.mossAngleVariation, m_currentMoss.mossAngleVariation);
            float angleX = Random.Range(-m_currentMoss.mossAngleVariation, m_currentMoss.mossAngleVariation);
            float angleY = Random.Range(0, 360);
            childMoss.transform.RotateAround(childMoss.transform.position, up, angleY);
            childMoss.transform.RotateAround(childMoss.transform.position, right, angleX);
            childMoss.transform.RotateAround(childMoss.transform.position, forward, angleZ);
            childMoss.transform.SetParent(parentMoss.transform, worldPositionStays: true);
        }  
    }

    public int GetMossAmount()
    {
        return m_ParentMossObjects.Count;
    }

    public void SetMoss(MossData mossData)
    {
        m_currentMoss = mossData;
    }
}

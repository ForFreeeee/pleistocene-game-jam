using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using FMOD;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms;
using System.Linq;

public class MossShooter : MonoBehaviour
{
    [SerializeField]
    MossData m_currentMoss;
    [SerializeField]
    GameObject sceneLight;

    [SerializeField] MossManageur gameManager;
    [SerializeField]
    Vector3 m_mossScale;

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

    [SerializeField]
    EventReference m_mainTheme;
    [SerializeField]
    EventReference m_mossGrowing;
    [SerializeField]
    EventReference m_ambience;

    Dictionary<string, FMOD.Studio.EventInstance> eventInstances;

    Dictionary<string, FMODUnity.StudioEventEmitter> m_emitters; 

    public void CreateEventInstance(string eventName, EventReference eventReference)
    {
        FMOD.Studio.EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        if(eventName=="MossGrowth")
        {
            eventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
        }
        eventInstances.Add(eventName, eventInstance);
    }

    public void PlayEventInstance(string eventName)
    {
        if (m_emitters.TryGetValue(eventName, out FMODUnity.StudioEventEmitter emitter))
        {
            emitter.Play();
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Emitter for '{eventName}' not found.");
        }
    }

    public void StopEventInstance(string eventName)
    {
        if (m_emitters.TryGetValue(eventName, out FMODUnity.StudioEventEmitter emitter))
        {
            emitter.Stop();
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Emitter for '{eventName}' not found.");
        }
    }

    public void SetParameter(string eventName, string parameterName, float value)
    {
        if (m_emitters.TryGetValue(eventName, out FMODUnity.StudioEventEmitter emitter))
        {
            UnityEngine.Debug.Log($"Setting parameter '{parameterName}' to {value} on event '{eventName}'.");
            emitter.SetParameter(parameterName, value);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Emitter for '{eventName}' not found.");
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_ParentMossObjects = new List<GameObject>();
        m_isSpraying = false;

        eventInstances = new Dictionary<string, EventInstance>();

        m_emitters = new Dictionary<string, StudioEventEmitter>();
        FMODUnity.StudioEventEmitter[] emitters_array = GetComponents<FMODUnity.StudioEventEmitter>();
        EventDescription ed;
        for (int i = 0; i < emitters_array.Length; i++)
        {
            m_emitters.Add(emitters_array[i].EventReference.Path, emitters_array[i]);
        }


        CreateEventInstance("MainTheme", m_mainTheme);
        PlayEventInstance("event:/MainTheme");
        CreateEventInstance("MossGrowth", m_mossGrowing);
        PlayEventInstance("event:/MossGrowth");
        CreateEventInstance("Ambience", m_ambience);
        PlayEventInstance("event:/Ambience");
    }

    // Update is called once per frame
    void Update()
    {
        m_currentDelay -= Time.deltaTime;
        Vector2 val = Mouse.current.position.ReadValue();
        //UnityEngine.Debug.Log(val);

        if (m_currentDelay < 0 && m_isSpraying)
        {
            Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(val.x, val.y, Camera.main.nearClipPlane));
            Ray r = new Ray(Camera.main.transform.position, point - Camera.main.transform.position);
            RaycastHit hit;
            UnityEngine.Debug.DrawRay(r.origin, r.direction * 100, Color.red, 3.0f);
            if (Physics.Raycast(r, out hit))
            {
                //UnityEngine.Debug.Log("Hit: " + hit.collider.gameObject.name);
                if (CanPlaceParentMoss(hit.collider.gameObject.tag) == false)
                {
                    return;
                }
                
                SetParameter("event:/MossGrowth", "CanGrow", 1);
                GenerateMoss(hit, hit.collider.gameObject);
            } 
            m_currentDelay = m_setDelayValue;
        }
    }

    bool CanPlaceParentMoss(string hitTag)
    {
        if(hitTag=="OutOfBounds")
        {
            return false;
        }
        return true;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        m_isSpraying = context.ReadValueAsButton();  
    }

    void GenerateMoss(RaycastHit hit, GameObject hit_object) {
       Vector3 hitpoint = hit.point;
        Vector3 normal = hit.normal;
        GameObject parentMoss = Instantiate(m_currentMoss.parentMossPrefab, hitpoint, Quaternion.identity);
        MossController mossController=parentMoss.GetComponent<MossController>();
        mossController.hitPoint=hit.point;
        mossController.textureCoord=hit.textureCoord;
        mossController.hitObject=hit_object;
        MossCoverable moss_coverable;
        mossController.mossCoverable= hit.collider.gameObject.TryGetComponent<MossCoverable>(out moss_coverable) ? moss_coverable : null;
        mossController.directionalLight=sceneLight;
        mossController.gameManageur=gameManager;
        parentMoss.transform.localScale = m_mossScale;
        parentMoss.GetComponent<MossController>().ShouldMossLive(hit_object.tag);
        parentMoss.transform.rotation = Quaternion.FromToRotation(parentMoss.transform.up, hit.normal); 

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
            Vector3 localScaleBeforeParent = childMoss.transform.localScale;
            childMoss.transform.SetParent(parentMoss.transform, worldPositionStays: true);
            childMoss.transform.localScale = localScaleBeforeParent;
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

using Unity.VisualScripting;
using UnityEngine;

public class MossController : MonoBehaviour
{
    public MossStateMachine MossStateMachine;
    public Vector3 hitPoint;
    public Vector2 textureCoord;
    public GameObject hitObject;
     public MossData MossData;
    public GameObject directionalLight;
    public MossManageur gameManageur;
    public MossCoverable mossCoverable;

    [SerializeField]
    private bool willDie;

    private void Awake()
    {
        MossStateMachine = new MossStateMachine(this);
    }

    private void Start()
    {
        MossStateMachine.Initialize(MossStateMachine.idleState);
    }

    private void Update()
    {
        MossStateMachine.GraphicsUpdate();
    }

    private void FixedUpdate()
    {
        MossStateMachine.PhysicsUpdate();
    }
    
public void ShouldMossLive(string hitTag)
     {
        willDie=false;
        Ray ray = new Ray(transform.position, directionalLight.transform.position - transform.position);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        bool hitSun = hits!= null && hits[0].collider.gameObject.tag == "Sun";
        //Debug.Log(hitSun + " " + hits[0].collider.gameObject.name);
        /*for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("Hit: " + hits[i].collider.gameObject.name);
        }*/
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 3.0f);
        if (!hitSun && MossData.LikesSun)
        {
            Debug.Log("Can't place moss here, No direct sunlight");
            willDie=true;
        }
        if (hitSun && !MossData.LikesSun)
        {
            Debug.Log("Can't place moss here, Too much sunlight");
            willDie=true;
        }
        if(MossData.LikesGround && hitTag!="Ground")
        {
            Debug.Log("Can't place moss here, Need ground");
            willDie=true;
        }
        if(MossData.LikesWater && hitTag!="Water")
        {
            Debug.Log("Can't place moss here, Need water");
            willDie=true;   
        }
        if(MossData.LikesRock && hitTag!="Rock")
        {
            Debug.Log("Can't place moss here, Need rock");
            willDie=true;
        }
    }

    public bool mustDie()
    {
        return willDie;
    }

}
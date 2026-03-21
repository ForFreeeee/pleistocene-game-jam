using Unity.VisualScripting;
using UnityEngine;

public class MossController : MonoBehaviour
{
    public MossStateMachine MossStateMachine;
    public MossData MossData;

    [SerializeField]
    private bool willDie;

    public CapsuleCollider MossCollider;
    public MeshRenderer MossMeshRenderer;


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
    
    public void AnticipateDeath()
    {
        willDie=true;
    }

    public bool mustDie()
    {
        return willDie;
    }

}
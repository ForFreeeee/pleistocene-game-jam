using Unity.VisualScripting;
using UnityEngine;

public class MossController : MonoBehaviour
{
    private MossStateMachine mossStateMachine;
    [SerializeField] private MossData mossData;

    private void Awake()
    {
        mossStateMachine = new MossStateMachine(this);
    }

    private void Start()
    {
        mossStateMachine.Initialize(mossStateMachine.idleState);
    }

    private void Update()
    {
        mossStateMachine.GraphicsUpdate();
    }

    private void FixedUpdate()
    {
        mossStateMachine.PhysicsUpdate();
    }

}
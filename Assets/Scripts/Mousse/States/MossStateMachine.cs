using System;
using UnityEngine;
using GameDev.Moss.States;

[Serializable]
public class MossStateMachine
{
    public IState CurrentState { get; private set; }
    public IdleState idleState;
    public GrowthState growthState;
    public DeathState deathState;
    public MatureState matureState;

    public MossStateMachine(MossController mossController)
    {
        this.idleState = new IdleState(mossController);
        this.growthState = new GrowthState(mossController);
        this.deathState = new DeathState(mossController);
        this.matureState = new MatureState(mossController);
    }

    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        startingState.Enter();
    }
    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();
    }

    public void GraphicsUpdate()
    {
        if (CurrentState != null)
        {
            //Debug.Log(CurrentState.ToString());
            CurrentState.GraphicsUpdate();
        }
    }

    public void PhysicsUpdate()
    {
        if (CurrentState != null && CurrentState != matureState)
        {
            CurrentState.PhysicsUpdate();
        }
    }
}
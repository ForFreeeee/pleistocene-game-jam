using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDev.Moss.States
{
    public class GrowthState : IState
    {
        private MossController moss;
        float growthTimer;

        // pass in any parameters you need in the constructors
        public GrowthState(MossController moss)
        {
            this.moss = moss;
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            // Debug.Log("Entering Idle State");
            growthTimer=moss.MossData.GrowthTime;
        }

        // per-frame logic, include condition to transition to a new state
        public void GraphicsUpdate()
        {
            float t=1-(growthTimer/moss.MossData.GrowthTime);
            growthTimer-=Time.deltaTime;
            moss.transform.localScale=Vector3.Lerp(moss.transform.localScale, moss.MossData.finalGrowthSize, t);
            if (growthTimer <= 0)
            {
                float covered_surface = moss.moss_coverable.AddMoss(moss.hitPoint, moss.textureCoord);
                moss.gameManageur.UpdateCoveredSurface(covered_surface, moss.hitObject);
                moss.MossStateMachine.TransitionTo(moss.MossStateMachine.matureState);
                return;
            }
        }

        public void PhysicsUpdate()
        {
            
        }

        public void Exit()
        {
            // code that runs when we exit the state
            //Debug.Log("Exiting Idle State");
        }
    }
}

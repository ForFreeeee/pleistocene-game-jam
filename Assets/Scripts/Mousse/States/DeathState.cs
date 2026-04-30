using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameDev.Moss.States
{
    public class DeathState : IState
    {
        private MossController moss;
        float decayTimer;

        // pass in any parameters you need in the constructors
        public DeathState(MossController moss)
        {
            this.moss = moss;
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            // Debug.Log("Entering Idle State");
            decayTimer=moss.MossData.DecayTime;
        }

        // per-frame logic, include condition to transition to a new state
        public void GraphicsUpdate()
        {
            float t=1-(decayTimer/moss.MossData.DecayTime);
            decayTimer-=Time.deltaTime;
            moss.gameObject.transform.localScale=Vector3.Lerp(moss.gameObject.transform.localScale, Vector3.zero, t);
            if (decayTimer <= 0)
            {
                GameObject.Destroy(moss.gameObject);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDev.Moss.States
{
    public class DeathState : IState
    {
        private MossController moss;

        // pass in any parameters you need in the constructors
        public DeathState(MossController moss)
        {
            this.moss = moss;
        }

        public void Enter()
        {
            // code that runs when we first enter the state
            // Debug.Log("Entering Idle State");
        }

        // per-frame logic, include condition to transition to a new state
        public void GraphicsUpdate()
        {

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

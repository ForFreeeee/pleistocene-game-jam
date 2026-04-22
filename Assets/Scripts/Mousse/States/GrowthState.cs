using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace GameDev.Moss.States
{
    public class GrowthState : IState
    {
        private MossController moss;
        float growthTimer;
        Vector3[] finalGrowthSizes;
        List<GameObject> children = new List<GameObject>();

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
            int childAmount = 0;
            for (int i = 0; i < moss.gameObject.transform.childCount; i++)
            {
                if (!moss.gameObject.transform.GetChild(i).name.Contains("Mesh"))
                {
                    childAmount++;
                    children.Add(moss.gameObject.transform.GetChild(i).gameObject);
                }
            }
            finalGrowthSizes = new Vector3[childAmount];
            for (int i = 0; i < childAmount; i++)
            {
                finalGrowthSizes[i] = new Vector3(Random.Range(moss.MossData.finalGrowthSize.x-moss.MossData.GrowthSizeVariation.x, moss.MossData.finalGrowthSize.x+moss.MossData.GrowthSizeVariation.x), 
                Random.Range(moss.MossData.finalGrowthSize.y-moss.MossData.GrowthSizeVariation.y, moss.MossData.finalGrowthSize.y+moss.MossData.GrowthSizeVariation.y),
                Random.Range(moss.MossData.finalGrowthSize.z-moss.MossData.GrowthSizeVariation.z, moss.MossData.finalGrowthSize.z+moss.MossData.GrowthSizeVariation.z));
            }
        }

        // per-frame logic, include condition to transition to a new state
        public void GraphicsUpdate()
        {
            float t=1-(growthTimer/moss.MossData.GrowthTime);
            growthTimer-=Time.deltaTime;
            for (int i = 0; i < finalGrowthSizes.Length; i++)
            {
                children[i].transform.localScale=Vector3.Lerp(children[i].transform.localScale, finalGrowthSizes[i], t);
            }
            if (growthTimer <= 0)
            {
                float covered_surface = 0f;
                if (moss.mossCoverable != null)
                {
                    covered_surface = moss.mossCoverable.AddMoss(moss.hitPoint, moss.textureCoord);
                }
                else
                {
                    covered_surface=0.5f;
                }
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

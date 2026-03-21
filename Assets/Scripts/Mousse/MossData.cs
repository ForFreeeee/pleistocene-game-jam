using UnityEngine;

[CreateAssetMenu(fileName = "MossData", menuName = "Scriptable Objects/MossData")]
public class MossData : ScriptableObject
{
   public float GrowthTime;
   public float DecayTime;
   public bool LikesWater;
   public bool LikesSun;
   public bool LikesRock;
   public bool LikesGround; 
}

using UnityEngine;

[CreateAssetMenu(fileName = "MossData", menuName = "Scriptable Objects/MossData")]
public class MossData : ScriptableObject
{
    public int id;
   public float GrowthTime;
   public float DecayTime;
   public bool LikesWater;
   public bool LikesSun;
   public bool LikesRock;
   public bool LikesGround;
   public Vector3 finalGrowthSize;
    public float mossRadius;
    public float mossRadiusShift;
    public float mossAngleVariation;
    public int mossDensity;
    public float mossDensityVariation;

    public string NameKey;
    public string ShortDescriptionKey;
    public string DetailedKey;
    public Sprite sprite;
    public GameObject parentMossPrefab;
    public GameObject childMossPrefab;
}

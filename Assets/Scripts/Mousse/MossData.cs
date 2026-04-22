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
   public Vector3 GrowthSizeVariation;

    public string NameKey;
    public string ShortDescriptionKey;
    public string DetailedKey;
    public Sprite spritePhoto;
    public Sprite spriteColor;
    public GameObject parentMossPrefab;
    public GameObject childMossPrefab;

    public float mossRadius;
    public float mossRadiusShift;
    public float mossAngleVariation;
    public int mossDensity;
    public float mossDensityVariation;
}

using UnityEngine;

public class MossCoverable : MonoBehaviour
{
    [SerializeField]
    Material m_mossMaskLayer;

    Material m_mossMaskLayerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_mossMaskLayerRef = new Material(m_mossMaskLayer);
        GetComponent<MeshRenderer>().material = m_mossMaskLayerRef;
        
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Sin(Time.time);
        m_mossMaskLayer.SetFloat("AlphaMask", alpha);
    }
}

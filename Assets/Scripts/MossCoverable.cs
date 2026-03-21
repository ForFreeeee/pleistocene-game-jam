using System;
using UnityEngine;

public class MossCoverable : MonoBehaviour
{
    [SerializeField]
    Material m_mossMaskLayer;

    [SerializeField]
    Texture2D m_objectTexture;
    [SerializeField]
    Texture2D m_mossTexture;

    Texture2D m_maskMoss;

    Material m_mossMaskLayerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_mossMaskLayerRef = new Material(m_mossMaskLayer);

        // TEXTURE
        m_maskMoss = new Texture2D(m_objectTexture.width, m_objectTexture.height);
        for (int i = 0; i < m_objectTexture.width; i++)
            for (int j = 0; j < m_objectTexture.height; j++)
            {
                m_maskMoss.SetPixel(i, j, Color.black);

            }
        m_maskMoss.Apply();

        m_mossMaskLayerRef.SetTexture("_MossTexture", m_mossTexture);
        m_mossMaskLayerRef.SetTexture("_ObjectTexture", m_objectTexture);
        m_mossMaskLayerRef.SetTexture("_MaskTexture", m_maskMoss);
        GetComponent<MeshRenderer>().material = m_mossMaskLayerRef;
    }

    internal void AddMoss(Vector3 point, Vector2 textureCoord)
    {
        Debug.Log("Adding moss");
        int u = Mathf.FloorToInt(textureCoord.x * m_maskMoss.width);
        int v = Mathf.FloorToInt(textureCoord.y * m_maskMoss.height);
       // for (int i = 0; i < m_maskMoss.width; i++)
        //    for (int j = 0; j < m_maskMoss.height; j++)

        for (int i = -50; i < 50; i++)
            for (int j = -50; j < 50; j++) {
                int ui = (u + i) % m_maskMoss.width;
                if (ui < 0) ui = m_maskMoss.width - ui;
                int vj = (v + j) % m_maskMoss.height;
                if (vj < 0) vj = m_maskMoss.height - vj;
                m_maskMoss.SetPixel(ui, vj, Color.white);
        }
        //m_maskMoss.SetPixel(u, v, Color.black);
        m_maskMoss.Apply();
        //m_mossMaskLayerRef.SetTexture("_MaskTexture", m_maskMoss);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

  
}

using System;
using UnityEngine;

public class MossCoverable : MonoBehaviour
{
    [SerializeField]
    Material m_mossMaskLayer;

    [SerializeField]
    Texture2D m_mossTexture;

    [SerializeField]
    ComputeShader m_mossRemover;
    [SerializeField]
    ComputeShader m_mossInit;
    RenderTexture m_renderTexture;

    Texture2D m_maskMoss;

    Material m_mossMaskLayerRef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_mossMaskLayerRef = new Material(m_mossMaskLayer);
        // TEXTURE
        m_maskMoss = new Texture2D(2048, 2048);
        m_renderTexture = new RenderTexture(2048, 2048, 1);
        m_renderTexture.enableRandomWrite = true;
        m_renderTexture.Create();
        /*for (int i = 0; i < m_objectTexture.width; i++)
            for (int j = 0; j < m_objectTexture.height; j++)
            {
                m_maskMoss.SetPixel(i, j, Color.black);

            }
        m_maskMoss.Apply();*/
        m_mossInit.SetTexture(m_mossInit.FindKernel("CSInit"), Shader.PropertyToID("Result"), m_renderTexture);
        m_mossInit.Dispatch(m_mossInit.FindKernel("CSInit"), Mathf.CeilToInt(m_maskMoss.width / 8f), Mathf.CeilToInt(m_maskMoss.height / 8f), 1);

        m_mossMaskLayerRef.SetTexture("_MossTexture", m_mossTexture);
        //m_mossMaskLayerRef.SetTexture("_ObjectTexture", m_objectTexture);
        m_mossMaskLayerRef.SetTexture("_MaskTexture", m_renderTexture);
        GetComponent<MeshRenderer>().material = m_mossMaskLayerRef;
    }

    internal void AddMoss(Vector3 point, Vector2 textureCoord)
    {
        Debug.Log("Adding moss");
        Debug.Log(textureCoord);
        m_mossRemover.SetFloats(Shader.PropertyToID("HitUV"), textureCoord.x, textureCoord.y);
        m_mossRemover.SetFloat(Shader.PropertyToID("Step"), 2f);
        m_mossRemover.SetInts(Shader.PropertyToID("Res"), 2048, 2048);
        m_mossRemover.SetInts(Shader.PropertyToID("HitXY"), Mathf.FloorToInt(textureCoord.x * 2048), Mathf.FloorToInt(textureCoord.y * 2048));
        m_mossRemover.SetTexture(m_mossRemover.FindKernel("CSMain"), Shader.PropertyToID("Result"), m_renderTexture);
        m_mossRemover.Dispatch(m_mossRemover.FindKernel("CSMain"), Mathf.CeilToInt(m_maskMoss.width / 8f), Mathf.CeilToInt(m_maskMoss.height / 8f), 1);
        //m_mossMaskLayerRef.SetTexture("_MaskTexture", m_maskMoss);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

  
}

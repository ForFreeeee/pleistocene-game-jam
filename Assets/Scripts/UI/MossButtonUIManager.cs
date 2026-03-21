using System;
using System.Collections.Generic;
using UnityEngine;

public class MossButtonUIManager : MonoBehaviour
{
    [SerializeField]
    Transform contentTransform;

    [SerializeField]
    ExplainCanvas explainCanvas;

    [SerializeField]
    MossButton mossButtonPrefab;



    internal void Init(List<int> listMoss, Action<int> changeSelectedMoss)
    {
        for (int i = 0; i < listMoss.Count; i++)
        {
            mossButtonPrefab.Init(i, changeSelectedMoss);
            Instantiate(mossButtonPrefab, contentTransform);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeSelectedMoss(int newMoss)
    {
        explainCanvas.SetState(newMoss);
    }
}

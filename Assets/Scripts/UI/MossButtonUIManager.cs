using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public class MossButtonUIManager : MonoBehaviour
{
    [SerializeField]
    Transform contentTransform;

    [SerializeField]
    ExplainCanvas explainCanvas;

    [SerializeField]
    MossButton mossButtonPrefab;

    internal void Init(List<MossData> listMoss, Action<int> changeSelectedMoss)
    {
        foreach (var moss in listMoss)
        {
            var button = Instantiate(mossButtonPrefab, contentTransform);
            button.Init(moss.id,moss.sprite, changeSelectedMoss);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChangeSelectedMoss(MossData newMoss)
    {
        explainCanvas.UpdateMossData(newMoss);
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MossManageur : MonoBehaviour
{
    int selectedMossId;
    [SerializeReference]
    List<MossData> mossList;

    [SerializeField]
    MossButtonUIManager mossButtonUIManager;

    void Start()
    {
        selectedMossId = -1;

        mossButtonUIManager.Init(mossList, ChangeSelectedMoss, ChangeHoverMoss);

        ChangeSelectedMoss(selectedMossId);
    }

    void ChangeSelectedMoss(int newMoss)
    {
        selectedMossId = newMoss;
    }
    void ChangeHoverMoss(int newMoss)
    {
        mossButtonUIManager.ChangeHoverMoss(mossList.Where(m => m.id == newMoss).FirstOrDefault());
    }

    public void UpdatePlaceToPlaceProperties(MossData mossData)
    {
        mossButtonUIManager.UpdatePlaceToPlaceProperties(mossData);
    }

    public void UpdateLikeSunImageProperties(MossData mossData)
    {
        mossButtonUIManager.UpdateLikeSunImageProperties(mossData);
    }

    public void LauchEndGame()
    {
        mossButtonUIManager.ShowEndGameScreen();
    }
}

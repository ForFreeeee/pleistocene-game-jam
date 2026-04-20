using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MossManageur : MonoBehaviour
{
    [SerializeField]
    int selectedMossId;
    [SerializeReference]
    List<MossData> mossList;

    [SerializeField]
    MossButtonUIManager mossButtonUIManager;

    [SerializeField]
    MossShooter mossShooter;

    [SerializeField]
    int mossAmountWinCondition;
    [SerializeField]
    float surfaceCoveredWinCondition;
    public float surfaceCovered;

    void Start()
    {
        selectedMossId = 0;

        mossButtonUIManager.Init(mossList, ChangeSelectedMoss, ChangeHoverMoss);

        ChangeSelectedMoss();
    }

    void Update()
    {
        if(mossShooter.GetMossAmount() >= mossAmountWinCondition && surfaceCovered >= surfaceCoveredWinCondition)
        {
            LauchEndGame();
        }
    }

    void ChangeSelectedMoss()
    {
        selectedMossId = mossButtonUIManager.GetSelectedMooss();
        Debug.Log(selectedMossId);
        SetSelectedMoss();
    }

     void SetSelectedMoss()
    {
        if(selectedMossId == -1)
        {
            return;
        }
        mossShooter.SetMoss(mossList[selectedMossId]);
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

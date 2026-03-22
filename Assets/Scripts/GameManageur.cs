using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManageur : MonoBehaviour
{
    int selectedMossId;
    [SerializeReference]
    List<MossData> mossList;

    [SerializeField]
    MossButtonUIManager mossButtonUIManager;

    [SerializeField]
    MossShooter mossShooter;

    [SerializeField]
    int mossAmountWinCondition;

    void Start()
    {
        selectedMossId = 0;
        SetSelectedMoss();

        mossButtonUIManager.Init(mossList, ChangeSelectedMoss, ChangeHoverMoss);

        ChangeSelectedMoss();
    }

    void Update()
    {
        if(mossShooter.GetMossAmount() >= mossAmountWinCondition)
        {
            LauchEndGame();
        }
    }

    void ChangeSelectedMoss()
    {
        selectedMossId = mossButtonUIManager.GetSelectedMooss();
        SetSelectedMoss();
    }

    void SetSelectedMoss()
    {
        foreach (MossData moss in mossList)
        {
            if (moss.id == selectedMossId)
            {
                mossShooter.SetMoss(moss);
            }
        }
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

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
    float[] m_neededToWin;
    [SerializeField]
    float[] m_coveredSurface;

    [SerializeField]
    GameObject[] m_toBeCovered;

    void Start()
    {
        selectedMossId = 0;

        mossButtonUIManager.Init(mossList, ChangeSelectedMoss, ChangeHoverMoss);

        ChangeSelectedMoss();

        m_coveredSurface = new float[m_toBeCovered.Length];
        for (int i = 0; i < m_coveredSurface.Length; i++)
            m_coveredSurface[i] = 0;
    }

    void ChangeSelectedMoss()
    {
        selectedMossId = mossButtonUIManager.GetSelectedMooss();
        //Debug.Log(selectedMossId);
        SetSelectedMoss();
    }

    bool CheckWinCond()
    {
        for (int i = 0; i < m_coveredSurface.Length; i++)
        {
            if (m_coveredSurface[i] < m_neededToWin[i])
                return false;
        }
        return true;
    }

    public void UpdateCoveredSurface(float coveredSurface, GameObject gameObject)
    {
        for (int i = 0; i < m_toBeCovered.Length; i++)
        {
            if (m_toBeCovered[i] == gameObject)
                m_coveredSurface[i] = coveredSurface;
        }

        float average_coverage = 0.0f;
        for (int i = 0; i < m_coveredSurface.Length; i++)
            average_coverage += m_coveredSurface[i];
        average_coverage = average_coverage / m_coveredSurface.Length;
        mossShooter.SetParameter("event:/MossGrowth", "CanGrow", 0);
        mossShooter.SetParameter("event:/MainTheme", "Completion", average_coverage/100.0f);
        if (CheckWinCond())
        {
            LaunchEndGame();   
        }
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

    public void LaunchEndGame()
    {
        mossButtonUIManager.ShowEndGameScreen();
    }

}

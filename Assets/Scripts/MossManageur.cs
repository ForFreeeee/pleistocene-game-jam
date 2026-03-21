using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MossManageur : MonoBehaviour
{
    int selectedMoss;
    //TMP
    [SerializeReference]
    List<int> listMoss = new List<int>() { 1,2,3,4};

    [SerializeField]
    MossButtonUIManager mossButtonUIManager;

    void Start()
    {
        // todo get list Moss

        selectedMoss = -1;

        mossButtonUIManager.Init(listMoss, ChangeSelectedMoss);

        
    }

    void ChangeSelectedMoss(int newMoss)
    {
        selectedMoss = newMoss;
        mossButtonUIManager.ChangeSelectedMoss(newMoss);
    }
    
}

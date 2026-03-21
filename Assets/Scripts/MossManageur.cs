using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MossManageur : MonoBehaviour
{
    int selectedMoss;
    [SerializeReference]
    List<MossData> listMoss;

    [SerializeField]
    MossButtonUIManager mossButtonUIManager;

    void Start()
    {

        selectedMoss = -1;

        mossButtonUIManager.Init(listMoss, ChangeSelectedMoss);

        ChangeSelectedMoss(selectedMoss);


    }

    void ChangeSelectedMoss(int newMoss)
    {
        selectedMoss = newMoss;
        mossButtonUIManager.ChangeSelectedMoss(listMoss.Where(m => m.id== newMoss).FirstOrDefault());
    }
    
}

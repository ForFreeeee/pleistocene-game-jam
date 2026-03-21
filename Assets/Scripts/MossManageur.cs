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

        mossButtonUIManager.Init(mossList, ChangeSelectedMoss);

        ChangeSelectedMoss(selectedMossId);
    }

    void ChangeSelectedMoss(int newMoss)
    {
        selectedMossId = newMoss;
        mossButtonUIManager.ChangeSelectedMoss(mossList.Where(m => m.id== newMoss).FirstOrDefault());
    }
}

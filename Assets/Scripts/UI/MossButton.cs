using System;
using UnityEngine;
using UnityEngine.UI;

public class MossButton : MonoBehaviour
{
    int id;
    [SerializeField]
    Image image;
    [SerializeField]
    Button button;

    public void Init(int id, Action<int> changeSelectedMoss)
    {
        this.id = id;
        //this.image = image;
        this.button.onClick.AddListener(delegate { changeSelectedMoss(id); });
    }
}

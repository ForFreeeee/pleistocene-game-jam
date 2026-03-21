using System;
using UnityEngine;
using UnityEngine.UI;

public class MossButton : MonoBehaviour
{
    int id;
    [SerializeField]
    Image image;
    [SerializeField]
    public Button button;

    public void Init(int id, Sprite sprite, Action<int> changeSelectedMoss)
    {
        this.id = id;
        this.image.sprite = sprite;
        //TODO over
        this.button.onClick.AddListener(() => changeSelectedMoss(id));
    }
}

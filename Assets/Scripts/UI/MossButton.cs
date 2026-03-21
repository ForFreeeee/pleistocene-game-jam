using System;
using UnityEngine;
using UnityEngine.EventSystems;
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

        //Hover event
        EventTrigger trigger = button.GetComponentInParent<EventTrigger>();

        if (trigger == null) trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter;

        entry.callback.AddListener((eventData) => { changeSelectedMoss(id); });

        trigger.triggers.Add(entry);
    }
}

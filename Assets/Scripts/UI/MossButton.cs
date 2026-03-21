using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MossButton : MonoBehaviour
{
    public int id;
    [SerializeField]
    Image image;
    [SerializeField]
    public Toggle toggle;

    [SerializeField]
    Image imageLikeSun;

    [SerializeField]
    Image imagePlaceToPlace;

    public void Init(int id, Sprite sprite, Action changeSelectedMoss, Action<int> changeHoverMoss)
    {
        this.id = id;
        this.image.sprite = sprite;

        //OnClick event
        this.toggle.onValueChanged.AddListener(delegate {
            changeSelectedMoss();});

        //Hover event
        EventTrigger trigger = toggle.GetComponentInParent<EventTrigger>();

        if (trigger == null) trigger = toggle.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter;

        entry.callback.AddListener((eventData) => { changeHoverMoss(id); });

        trigger.triggers.Add(entry);
    }

    public void UpdateLikeSunImage(Sprite newSprite)
    {
        imageLikeSun.sprite = newSprite;
    }

    public void UpdatePlaceToPlace(Sprite newSprite)
    {
        imagePlaceToPlace.sprite = newSprite;
    }
}

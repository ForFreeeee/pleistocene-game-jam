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

    [SerializeField]
    Image imageLikeSun;

    [SerializeField]
    Image imagePlaceToPlace;

    public void Init(int id, Sprite sprite, Action<int> changeSelectedMoss, Action<int> changeHoverMoss)
    {
        this.id = id;
        this.image.sprite = sprite;

        //OnClick event
        this.button.onClick.AddListener(() => changeSelectedMoss(id));

        //Hover event
        EventTrigger trigger = button.GetComponentInParent<EventTrigger>();

        if (trigger == null) trigger = button.gameObject.AddComponent<EventTrigger>();

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

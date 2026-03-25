using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MossButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    Action<int> changeHoverMoss;

    public void Init(int id, Sprite sprite, Action changeSelectedMoss, Action<int> changeHoverMoss)
    {
        this.id = id;
        this.image.sprite = sprite;

        //OnClick event
        this.toggle.onValueChanged.AddListener(delegate {
            changeSelectedMoss();});

        //Hover event
        this.changeHoverMoss = changeHoverMoss;
    }

    public void UpdateLikeSunImage(Sprite newSprite)
    {
        imageLikeSun.sprite = newSprite;
    }

    public void UpdatePlaceToPlace(Sprite newSprite)
    {
        imagePlaceToPlace.sprite = newSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        changeHoverMoss(id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        changeHoverMoss(-1);
    }
}

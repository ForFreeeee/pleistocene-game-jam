using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;

public class MossButtonUIManager : MonoBehaviour
{
    [SerializeField]
    Transform contentTransform;

    [SerializeField]
    ExplainCanvas explainCanvas;

    [SerializeField]
    MossButton mossButtonPrefab;

    [SerializeField]
    Sprite DefaultSprite;

    [SerializeField]
    Sprite likeSunSprite;

    [SerializeField]
    Sprite notLikeSunSprite;

    [SerializeField]
    Sprite rockSprite;

    [SerializeField]
    Sprite groundSprite;

    [SerializeField]
    Sprite waterSprite;

    Dictionary<int,MossButton> buttonList;

    internal void Init(List<MossData> listMoss, Action<int> changeSelectedMoss, Action<int> changeHoverMoss)
    {
        buttonList = new Dictionary<int,MossButton>();
        foreach (var moss in listMoss)
        {
            var button = Instantiate(mossButtonPrefab, contentTransform);
            button.Init(moss.id,moss.sprite, changeSelectedMoss, changeHoverMoss);
            button.UpdateLikeSunImage(DefaultSprite);
            button.UpdatePlaceToPlace(DefaultSprite);
            buttonList.Add(moss.id,button);
        }
    }

    public void ChangeHoverMoss(MossData newMoss)
    {
        explainCanvas.UpdateMossData(newMoss);
    }

    public void UpdatePlaceToPlaceProperties(MossData mossData)
    {
        var buttonToChange = buttonList.GetValueOrDefault(mossData.id);
        if (mossData.LikesGround)
        {
            buttonToChange?.UpdatePlaceToPlace(groundSprite);
        }
        else if (mossData.LikesRock)
        {
            buttonToChange?.UpdatePlaceToPlace(rockSprite);
        }else
        {
            buttonToChange?.UpdatePlaceToPlace(waterSprite);
        }
    }

    public void UpdateLikeSunImageProperties(MossData mossData)
    {
        var buttonToChange = buttonList.GetValueOrDefault(mossData.id);
        if (mossData.LikesSun)
        {
            buttonToChange?.UpdateLikeSunImage(likeSunSprite);
        }
        else
        {
            buttonToChange?.UpdateLikeSunImage(notLikeSunSprite);
        }
    }

}

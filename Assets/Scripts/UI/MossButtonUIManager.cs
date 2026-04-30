using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class MossButtonUIManager : MonoBehaviour
{
    [SerializeField]
    ToggleGroup toggleGroup;

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

    [SerializeField]
    GameObject endGameScreen;

    internal void Init(List<MossData> listMoss, Action changeSelectedMoss, Action<int> changeHoverMoss)
    {
        buttonList = new Dictionary<int,MossButton>();
        foreach (var moss in listMoss)
        {
            var button = Instantiate(mossButtonPrefab, toggleGroup.transform);
            button.Init(moss.id,moss.spriteColor, changeSelectedMoss, changeHoverMoss, toggleGroup);
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

    public void ShowEndGameScreen()
    {
        endGameScreen.SetActive(true);
    }

    public int GetSelectedMooss()
    {
        foreach (var kvp in buttonList)
    {
        if (kvp.Value.toggle.isOn)
        {
            return kvp.Key;
        }
    }
    return -1;
    }

}

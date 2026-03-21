using TMPro;
using UnityEngine;
using UnityEngine.Localization;

public class ExplainCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    TextMeshProUGUI NameText;

    [SerializeField]
    TextMeshProUGUI DetailedText;


    LocalizedString localizedName;
    LocalizedString localizedDetailed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //localizedName.StringChanged += ValueNameChanged;
        //localizedDetailed.StringChanged += ValueDetailedChanged;

        //TODO localized
        //localizedName = new LocalizedString();
        //localizedName.TableReference = "UI-trad";
    }


    public void UpdateMossData(MossData? mossData)
    {
        if (mossData == null)
        {
            Show(false);
        }
        else
        {
            Show(true);
            NameText.text = mossData.NameKey;
            DetailedText.text = mossData.DetailedKey;
        }
    }

    private void Show( bool show)
    {
        //anim here todo
        content.SetActive(show);

    }



    void ValueNameChanged(string value)
    {
        NameText.text = value;
    }

    
    void ValueDetailedChanged(string value)
    {
        DetailedText.text = value;
    }
}

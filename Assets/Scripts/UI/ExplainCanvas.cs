using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

public class ExplainCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject content;

    [SerializeField]
    TextMeshProUGUI NameText;

    [SerializeField]
    TextMeshProUGUI ShortDescriptionText;

    [SerializeField]
    TextMeshProUGUI DetailedText;

    [SerializeField]
    Image image;


    LocalizedString localizedName;
    LocalizedString localizedDetailed;
    LocalizedString localizedShortDescription;

    const string tableRefName = "Ui-trad";

    void Start()
    {
        localizedName = new LocalizedString();
        localizedName.TableReference = tableRefName;
        localizedName.StringChanged += ValueNameChanged;

        localizedShortDescription = new LocalizedString();
        localizedShortDescription.TableReference = tableRefName;
        localizedShortDescription.StringChanged += ValueShortDescriptionChanged;

        localizedDetailed = new LocalizedString();
        localizedDetailed.TableReference = tableRefName;
        localizedDetailed.StringChanged += ValueDetailedChanged;

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
            localizedName.TableEntryReference = mossData.NameKey;
            localizedName.RefreshString();
            NameText.text = localizedName.GetLocalizedString();

            localizedShortDescription.TableEntryReference = mossData.ShortDescriptionKey;
            localizedShortDescription.RefreshString();
            ShortDescriptionText.text = localizedShortDescription.GetLocalizedString();

            localizedDetailed.TableEntryReference = mossData.DetailedKey;
            localizedDetailed.RefreshString();
            DetailedText.text = localizedName.GetLocalizedString();

            image.sprite = mossData.sprite;
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

    void ValueShortDescriptionChanged(string value)
    {
        ShortDescriptionText.text = value;
    }

    void ValueDetailedChanged(string value)
    {
        DetailedText.text = value;
    }
}

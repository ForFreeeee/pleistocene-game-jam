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

    const string tableRefName = "Ui-trad";

    void Start()
    {
        localizedName = new LocalizedString();
        localizedName.TableReference = tableRefName;
        localizedName.StringChanged += ValueNameChanged;

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

            localizedDetailed.TableEntryReference = mossData.DetailedKey;
            localizedDetailed.RefreshString();
            DetailedText.text = localizedName.GetLocalizedString();
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

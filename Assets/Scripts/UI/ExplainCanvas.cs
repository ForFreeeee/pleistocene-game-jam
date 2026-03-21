using UnityEngine;

public class ExplainCanvas : MonoBehaviour
{

    GameObject content;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(int id)
    {
        if (id == -1)
        {
            Show(false);
        }
        else
        {
            Show(true);
        }
    }

    private void Show( bool show)
    {
        //anim here todo
        content.SetActive(show);

    }
}

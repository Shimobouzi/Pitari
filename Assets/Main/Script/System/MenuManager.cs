using UnityEngine;
public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TitlePanel;
    [SerializeField]
    private GameObject OptionPanel;

    private void Start()
    {
        TitlePanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    public void OnStart()
    {
    }

    public void OnOption()
    {
        TitlePanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void OnBack()
    {
        TitlePanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

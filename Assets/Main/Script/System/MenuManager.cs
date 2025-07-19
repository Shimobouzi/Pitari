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
        PitariSceneManager.Instance.ToStage1();
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
    public void OnJoycon() //and KeyBord
    {
        PitariDB.Instance.OnJoycon();
    }

    public void OnOricon()
    {
        PitariDB.Instance.OnOricon();
    }
    public void OnPrecon()
    {
        PitariDB.Instance.OnPrecon();
    }
    public void Japanese()
    {
        PitariDB.Instance.Japanese();
    }
    public void English()
    {
        PitariDB.Instance.English();
    }
}

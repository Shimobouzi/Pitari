using UnityEngine;
using UnityEngine.SceneManagement;


public class PitariSceneManager : MonoBehaviour
{
    public static PitariSceneManager Instance;
    public static bool isClear;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            ToTitle();
        }
    }

    public void ToTitle()
    {
        Time.timeScale = 1.0f;
        AudioManager.StopBgm();
        AudioManager.PlayBgm("bgmT");
        SceneManager.LoadScene("Title");
    }

    public void ToResult(bool isClear_)
    {
        isClear = isClear_;
        Time.timeScale = 1.0f;
        AudioManager.StopBgm();
        SceneManager.LoadScene("Result");
    }


    public void ToMain()
    {
        Time.timeScale = 1.0f;
        AudioManager.StopBgm();
        SceneManager.LoadScene("Main");
    }


    public void ToStage1()
    {
        Time.timeScale = 1.0f;
        AudioManager.StopBgm();
        AudioManager.PlayBgm("bgmE");
        SceneManager.LoadScene("Stage1");
    }

    public void ToStage2()
    {
        Time.timeScale = 1.0f;
        AudioManager.StopBgm();
        AudioManager.PlayBgm("bgmG");
        SceneManager.LoadScene("Stage2");
    }

    public void ToStage3()
    {
        Time.timeScale = 1.0f;
        AudioManager.StopBgm();
        AudioManager.PlayBgm("bgmJ");
        SceneManager.LoadScene("Stage3");
    }


}

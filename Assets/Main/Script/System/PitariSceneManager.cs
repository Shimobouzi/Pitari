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

    public void ToTitle()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Title");
    }

    public void ToResult(bool isClear_)
    {
        isClear = isClear_;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Result");
    }


    public void ToMain()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main");
    }


    public void ToStage1()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Stage1");
    }

    public void ToStage2()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Stage2");
    }

    public void ToStage3()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Stage3");
    }


}

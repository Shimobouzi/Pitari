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
        SceneManager.LoadScene("Title");
    }

    public void ToResult(bool isClear_)
    {
        isClear = isClear_;
        SceneManager.LoadScene("Result");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main");
    }


}

using UnityEngine;
using UnityEngine.SceneManagement;


public class PitariSceneManager : MonoBehaviour
{
    public static PitariSceneManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void ToResult()
    {
        SceneManager.LoadScene("Result");
    }

    public void ToMain()
    {
        SceneManager.LoadScene("Main");
    }


}

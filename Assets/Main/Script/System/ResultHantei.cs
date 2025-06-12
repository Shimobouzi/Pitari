using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject Clear;
    public GameObject GameOver;
    void Start()
    {
        if(PitariSceneManager.isClear)
        {
            Clear.SetActive(true);
            GameOver.SetActive(false);
        }else
        {
            Clear.SetActive(false);
            GameOver.SetActive(true);
        }
    }


}

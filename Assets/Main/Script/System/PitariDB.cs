using UnityEngine;

public class PitariDB : MonoBehaviour
{
    private int conBool = 0;
    /// <summary>
    /// シングルトンインスタンス
    /// </summary>
    public static PitariDB Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnJoycon() //and KeyBord
    {
        Debug.Log("joycon");
        conBool = 0;
        JoyconManager.Instance.ReStart();
    }

    public void OnOricon()
    {
        Debug.Log("oricon");
        conBool = 1;
        OriconManager.instance.GetPicoHub();
    }
    public void OnPrecon()
    {
        conBool = 2;
    }

    public int GetConBool()
    {
        return conBool;
    }
}

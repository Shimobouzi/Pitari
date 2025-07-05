using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance;
    
    [SerializeField]
    private float gameTime = 180;
    private float passTime;
    private float meltTime;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        passTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        passTime += Time.deltaTime; 
    }

    public float GetMeltTime()
    {
        return (gameTime - passTime) / gameTime;
    }


}

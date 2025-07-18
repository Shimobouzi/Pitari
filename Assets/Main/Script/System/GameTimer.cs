using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer instance;

    [SerializeField]
    private float gameTime = 60f;
    private float passTime = 0f;
    private bool timerStarted = false;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!timerStarted) return;

        passTime += Time.deltaTime;
    }

    public void StartTimer()
    {
        timerStarted = true;
    }

    public float GetMeltTime()
    {
        return (gameTime - passTime) / gameTime;
    }
}

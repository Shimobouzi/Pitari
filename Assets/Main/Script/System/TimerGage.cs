using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerGage : MonoBehaviour
{
    [SerializeField]
    private Image timer;

    private bool isTimeUp = false;
    private bool timerStarted = false;

    private void Start()
    {
        // UIが表示されたらタイマー開始（ここで true にする）
        timerStarted = true;
        GameTimer.instance.StartTimer(); 
    }

    private void Update()
    {
        if (!timerStarted) return;

        float meltRatio = GameTimer.instance.GetMeltTime();
        timer.fillAmount = meltRatio;

        // 時間切れ判定
        if (!isTimeUp && meltRatio <= 0f)
        {
            isTimeUp = true;
            SceneManager.LoadScene("Result");
        }
    }
}

using UnityEngine.UI;
using UnityEngine;

public class TimerGage : MonoBehaviour
{
    [SerializeField]
    private Image timer;

    private void Update()
    {
        timer.fillAmount = GameTimer.instance.GetMeltTime();
    }
}

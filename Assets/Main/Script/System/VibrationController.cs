using UnityEngine;

public class VibrationController : MonoBehaviour
{
    public Joycon _joyconL;

    void Awake()
    {
        var joycons = JoyconManager.Instance.j;
        if (joycons.Count > 0)
        {
            _joyconL = joycons[0]; // ← Joy-Con 左（右なら joycons[1]）
            Debug.Log("Joy-Con 左を取得しました");
        }
        else
        {
            Debug.LogWarning("Joy-Con が見つかりませんでした");
        }
    }

    public void OnBuruburu()
    {
        if (_joyconL != null)
        {
            Debug.Log("振動開始");
            _joyconL.SetRumble(320, 640, 1.0f, 300);
            Invoke("StopRumble", 0.5f); // ← 少し余裕を持たせて確実に止める
        }
        else
        {
            Debug.LogWarning("joyconL が null です！");
        }
    }

    private void StopRumble()
    {
        Debug.Log("振動停止");
        _joyconL.SetRumble(0, 0, 0, 0);
    }
}

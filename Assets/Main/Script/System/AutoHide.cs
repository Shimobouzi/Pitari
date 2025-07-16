using UnityEngine;

public class AutoHide : MonoBehaviour
{
    [Header("非表示までの時間")]
    public float hideAfterSeconds = 5f;

    void Start()
    {
        Invoke(nameof(HideSelf), hideAfterSeconds);
    }

    void HideSelf()
    {
        gameObject.SetActive(false);
    }
}

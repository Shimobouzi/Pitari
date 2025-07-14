using System.Collections;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    [Header("煙アニメーション設定")]
    public Animator smokeAnimator;              // 煙のAnimator
    public string triggerName = "PlayKemuri";   // トリガー名（Animatorで設定）
    public float startDelay = 3.0f;             // ゲーム開始から煙再生までの待機時間

    //private bool hasPlayed = false;

    private void Start()
    {
        StartCoroutine(Kemuri());
    }

    IEnumerator Kemuri()
    {
        yield return new WaitForSeconds(14f);
        smokeAnimator.SetBool("IsSmoke", true);

    }
}    
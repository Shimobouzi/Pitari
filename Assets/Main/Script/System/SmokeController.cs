using System.Collections;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    [Header("煙アニメーション設定")]
    public Animator smokeAnimator;              // 煙のAnimator
    public string triggerName = "PlayKemuri";   // トリガー名（未使用）
    public float startDelay = 14.0f;            // 煙再生までの待機時間

    [Header("煙の効果音")]
    public AudioClip smokeSound;                // 煙の音
    private AudioSource audioSource;            // 音再生用のAudioSource

    private void Start()
    {
        // AudioSourceを追加
        audioSource = gameObject.AddComponent<AudioSource>();

        // コルーチン開始
        StartCoroutine(Kemuri());
    }

        IEnumerator Kemuri()
    {
        yield return new WaitForSeconds(startDelay);

        // 煙アニメーション開始
        smokeAnimator.SetBool("IsSmoke", true);

        // 音を再生（ピッチを下げてゆっくり再生）
        if (smokeSound != null)
        {
            audioSource.pitch = 0.8f; // 1.0が通常、0.8で少し遅くなる
            audioSource.PlayOneShot(smokeSound);
        }
    }

}

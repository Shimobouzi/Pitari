using System.Collections;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    [Header("煙アニメーション設定")]
    public Animator smokeAnimator;              // 煙のAnimator
    public float startDelay = 14.0f;            // 煙再生までの待機時間

    [Header("煙の効果音")]
    public AudioClip smokeSound;                // 煙の音
    public float soundDelay = 2.0f;             // サウンド再生までの追加待機時間
    private AudioSource audioSource;            // 音再生用のAudioSource

    private void Start()
    {
        // AudioSourceを追加
        audioSource = gameObject.AddComponent<AudioSource>();

        // 煙アニメーションのコルーチン開始
        StartCoroutine(PlaySmokeAnimation());

        // サウンド再生のコルーチン開始
        StartCoroutine(PlaySmokeSound());
    }

    IEnumerator PlaySmokeAnimation()
    {
        yield return new WaitForSeconds(startDelay);
        smokeAnimator.SetBool("IsSmoke", true);
    }

    IEnumerator PlaySmokeSound()
    {
        yield return new WaitForSeconds(startDelay + soundDelay);

        if (smokeSound != null)
        {
            audioSource.pitch = 0.8f;
            audioSource.PlayOneShot(smokeSound);
        }
    }
}

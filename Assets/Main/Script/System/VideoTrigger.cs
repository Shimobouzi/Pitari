using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement; // ← 追加
using System.Collections;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject videoCanvas;
    public CanvasGroup whiteFade;
    public AudioSource loopSound;
    public float fadeDuration = 3f;
    private bool hasPlayed = false;

    void Start()
    {
        videoCanvas.SetActive(false);
        whiteFade.alpha = 0f;
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            hasPlayed = true;
            StartCoroutine(PlayVideoWithFade());
        }
    }

    IEnumerator PlayVideoWithFade()
    {
        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            whiteFade.alpha = t / fadeDuration;
            yield return null;
        }

        videoCanvas.SetActive(true);
        videoPlayer.Play();
        loopSound.Play();

        for (float t = 0; t <= fadeDuration; t += Time.deltaTime)
        {
            whiteFade.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoCanvas.SetActive(false);
        loopSound.Stop();

        // タイトルシーンに戻る
        SceneManager.LoadScene("Title"); // ← シーン名を正確に！
    }
}

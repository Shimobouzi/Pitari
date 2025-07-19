using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShakeToStart : MonoBehaviour
{
    [SerializeField]
    private Image StartGage;
    [SerializeField]
    private GameObject gameScrean;

    //累積（ShakeToStart 内だけで完結）
    private float oriconBuffer = 0f;

    private float passTime = 0;

    private float maxTime = 3f;

    public Animator smokeAnimator;              // 煙のAnimator
    [Header("煙の効果音")]
    public AudioClip smokeSound;                // 煙の音
    private AudioSource audioSource;            // 音再生用のAudioSource

    private bool first = true;

    private void Start()
    {
        // AudioSourceを追加
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        if (PitariDB.Instance.GetConBool() == 0)
        {
            Joycon();
        }
        else if (PitariDB.Instance.GetConBool() == 1)
        {
            Oricon();
        }
    }

    public void ShakeGameStart(bool isStart)
    {
        if (isStart)
        {
            passTime += Time.deltaTime;
            StartGage.fillAmount = passTime / maxTime;
        }else if (!isStart && passTime > 0)
        {
            passTime -= Time.deltaTime;
            StartGage.fillAmount = passTime / maxTime;
        }

        if(passTime > maxTime && first)
        {
            first = false;
            StartCoroutine(nextScene());
        }
    }

    private void Joycon()
    {
        ShakeGameStart(Input_Player.Instance.RightMove_performed);
    }
    private void Oricon()
    {
        if (OriconManager.instance.pvcController())
            oriconBuffer += Time.deltaTime;             // ゲージの溜まり速度（大きいほど速く溜まる）
        else
            oriconBuffer -= Time.deltaTime * 0.4f;      // ゲージの減少速度（小さいほど減りにくい）

        oriconBuffer = Mathf.Clamp(oriconBuffer, 0f, maxTime);
        passTime = oriconBuffer;                        // そのままゲージに反映
        StartGage.fillAmount = passTime / maxTime;

        if (passTime >= maxTime && first)
        {
            first = false;
            StartCoroutine(nextScene());
        }
    }

    IEnumerator nextScene()
    {
        AsyncOperation stage = SceneManager.LoadSceneAsync("Stage1");
        stage.allowSceneActivation = false;

        // 煙アニメーション開始
        smokeAnimator.SetBool("IsSmoke", true);
        // 音を再生（ピッチを下げてゆっくり再生）
        if (smokeSound != null)
        {
            audioSource.pitch = 0.8f; // 1.0が通常、0.8で少し遅くなる
            audioSource.PlayOneShot(smokeSound);
        }
        yield return new WaitForSeconds(0.75f);
        gameScrean.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        AudioManager.StopBgm();
        AudioManager.PlayBgm("bgmE");
        stage.allowSceneActivation = true;
    }

}

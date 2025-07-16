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

    private float passTime = 0;

    private float maxTime = 3f;

    public Animator smokeAnimator;              // ����Animator
    [Header("���̌��ʉ�")]
    public AudioClip smokeSound;                // ���̉�
    private AudioSource audioSource;            // ���Đ��p��AudioSource

    private bool first = true;

    private void Start()
    {
        // AudioSource��ǉ�
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
        ShakeGameStart(OriconManager.instance.pvcController());
    }

    IEnumerator nextScene()
    {
        AsyncOperation stage = SceneManager.LoadSceneAsync("Stage1");
        stage.allowSceneActivation = false;

        // ���A�j���[�V�����J�n
        smokeAnimator.SetBool("IsSmoke", true);
        // �����Đ��i�s�b�`�������Ă������Đ��j
        if (smokeSound != null)
        {
            audioSource.pitch = 0.8f; // 1.0���ʏ�A0.8�ŏ����x���Ȃ�
            audioSource.PlayOneShot(smokeSound);
        }
        yield return new WaitForSeconds(0.75f);
        gameScrean.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        stage.allowSceneActivation = true;
    }

}

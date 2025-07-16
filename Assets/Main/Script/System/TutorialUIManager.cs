using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialUIManager : MonoBehaviour
{
    public static event System.Action OnTutorialClosed;

    [Header("UI要素")]
    public GameObject pausePanel;
    public GameObject speechBubble;
    public Image speechImage;

    [Header("狸関連")]
    public Transform tanuki;
    public Vector3 tanukiTargetPosition;
    public float tanukiMoveSpeed = 2f;
    private bool isTanukiMoving = false;
    private Vector3 tanukiStartPosition;

    [Header("画像設定")]
    public List<Sprite> tutorialSprites = new List<Sprite>();
    public float imageDisplayTime = 2.5f;

    [Header("サウンド（AudioClip）")]
    public AudioClip walkClip;
    public AudioClip bubbleClip;
    public AudioClip sitDownClip;

    private AudioSource audioSource;

    [Header("チュートリアル後の待機時間")]
    public float postTutorialDelay = 3f;

    [Header("ゲーム本編用")]
    public Camera tutorialCamera;
    public Camera playerCamera;
    public GameObject playerController;

    [Header("操作開始までの待機時間")]
    public float controlEnableDelay = 1.5f;

    [Header("カメラ切り替え時に変更するオブジェクト")]
    public GameObject objectToChange;
    public bool setObjectActive = true;

    private int tutorialCount = 0;

    void Start()
    {
        pausePanel.SetActive(false);
        speechBubble.SetActive(false);
        tanukiStartPosition = tanuki.position;
        tanuki.gameObject.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();

        if (speechImage == null)
        {
            Debug.LogError("speechImage が設定されていません！");
        }

        if (tutorialSprites.Count == 0)
        {
            Debug.LogWarning("tutorialSprites に画像が登録されていません！");
        }

        if (playerCamera != null) playerCamera.enabled = false;
        if (playerController != null) playerController.SetActive(false);
    }

    public void SetTutorialSprites(List<Sprite> newSprites)
    {
        tutorialSprites = newSprites;
    }

    public void ShowTutorial()
    {
        tanuki.position = tanukiStartPosition;
        pausePanel.SetActive(true);
        tanuki.gameObject.SetActive(true);

        SpriteRenderer sr = tanuki.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 10;
        }

        isTanukiMoving = true;

        if (walkClip != null)
        {
            audioSource.clip = walkClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (isTanukiMoving)
        {
            tanuki.position = Vector3.MoveTowards(tanuki.position, tanukiTargetPosition, tanukiMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(tanuki.position, tanukiTargetPosition) < 0.1f)
            {
                isTanukiMoving = false;

                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                if (sitDownClip != null)
                {
                    audioSource.PlayOneShot(sitDownClip);
                }

                StartCoroutine(ShowImageSequence());
            }
        }
    }

    IEnumerator ShowImageSequence()
    {
        speechBubble.SetActive(true);

        foreach (Sprite sprite in tutorialSprites)
        {
            speechImage.sprite = sprite;

            if (bubbleClip != null)
            {
                audioSource.PlayOneShot(bubbleClip);
            }

            yield return new WaitForSeconds(imageDisplayTime);
        }

        speechBubble.SetActive(false);
        pausePanel.SetActive(false);
        tanuki.gameObject.SetActive(false);

        OnTutorialClosed?.Invoke();

        tutorialCount++;

        if (tutorialCount >= 2)
        {
            yield return new WaitForSeconds(postTutorialDelay);
            StartGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("ゲーム本編スタート！");

        if (playerCamera != null) playerCamera.enabled = true;
        //if (tutorialCamera != null) tutorialCamera.enabled = false;

        // 🎯 カメラ切り替え時にオブジェクトを変更
        if (objectToChange != null)
        {
            objectToChange.SetActive(setObjectActive);
        }

        StartCoroutine(EnablePlayerControlAfterDelay(controlEnableDelay));
    }

    IEnumerator EnablePlayerControlAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (tutorialCamera != null) tutorialCamera.enabled = false;



        if (playerController != null)
        {
            playerController.SetActive(true);

            var moveScript = playerController.GetComponent<NewPlayerMove>();
            if (moveScript != null)
            {
                moveScript.enabled = true;
                Debug.Log("NewPlayerMove が有効になりました！");
            }
            else
            {
                Debug.LogWarning("NewPlayerMove スクリプトが見つかりません！");
            }
        }
    }
}

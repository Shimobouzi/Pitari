using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialUIManager_Second : MonoBehaviour
{
    public static event System.Action OnSecondTutorialClosed;

    [Header("UI要素")]
    public GameObject pausePanel;
    public GameObject speechBubble;
    public Image speechImage;

    [Header("狸関連")]
    public Transform tanuki;
    public Vector3 tanukiTargetPosition;
    public float tanukiMoveSpeed = 2f;
    private bool isTanukiMoving = false;

    [Header("画像設定")]
    public List<Sprite> tutorialSprites = new List<Sprite>();
    public float imageDisplayTime = 2.5f;

    private bool isTutorialRunning = false;

    void Start()
    {
        pausePanel.SetActive(false);
        speechBubble.SetActive(false);
        tanuki.gameObject.SetActive(false);
    }

    public void ShowTutorial()
    {
        if (isTutorialRunning) return;

        pausePanel.SetActive(true);
        tanuki.gameObject.SetActive(true);

        SpriteRenderer sr = tanuki.GetComponent<SpriteRenderer>();
        if (sr != null) sr.sortingOrder = 10;

        isTanukiMoving = true;
        isTutorialRunning = true;
    }

    void Update()
    {
        if (isTanukiMoving)
        {
            tanuki.position = Vector3.MoveTowards(tanuki.position, tanukiTargetPosition, tanukiMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(tanuki.position, tanukiTargetPosition) < 0.1f)
            {
                isTanukiMoving = false;
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
            yield return new WaitForSeconds(imageDisplayTime);
        }

        speechBubble.SetActive(false);
        pausePanel.SetActive(false);
        tanuki.gameObject.SetActive(false);
        isTutorialRunning = false;

        OnSecondTutorialClosed?.Invoke();
    }

    public void StartGame()
    {
        Debug.Log("ゲーム本編スタート！");
        // プレイヤー操作の有効化などをここに追加
    }
}

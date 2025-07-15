using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TutorialUIManager : MonoBehaviour
{
    public static event System.Action OnTutorialClosed;

    [Header("UI要素")]
    public GameObject pausePanel;         // ポーズ画面の背景パネル
    public GameObject speechBubble;       // 吹き出しUI
    public Image speechImage;             // 吹き出しに表示する画像

    [Header("狸関連")]
    public Transform tanuki;              // 狸のTransform
    public Vector3 tanukiTargetPosition;  // 狸の移動先
    public float tanukiMoveSpeed = 2f;    // 狸の移動速度
    private bool isTanukiMoving = false;

    [Header("画像設定")]
    public List<Sprite> tutorialSprites = new List<Sprite>(); // 表示する画像リスト
    public float imageDisplayTime = 2.5f; // 各画像の表示時間（秒）

    void Start()
    {
        pausePanel.SetActive(false);
        speechBubble.SetActive(false);
        tanuki.gameObject.SetActive(false);

        if (speechImage == null)
        {
            Debug.LogError("speechImage が設定されていません！");
        }

        if (tutorialSprites.Count == 0)
        {
            Debug.LogWarning("tutorialSprites に画像が登録されていません！");
        }
    }

    public void ShowTutorial()
    {
        pausePanel.SetActive(true);
        tanuki.gameObject.SetActive(true);

        SpriteRenderer sr = tanuki.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = 10;
        }

        isTanukiMoving = true;
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
            yield return new WaitForSeconds(imageDisplayTime); // 秒数で管理
        }

        speechBubble.SetActive(false);
        pausePanel.SetActive(false);
        tanuki.gameObject.SetActive(false);

        OnTutorialClosed?.Invoke();
    }

    public void StartGame()
        {
            Debug.Log("ゲーム本編スタート！");
            // 必要に応じて本編開始処理をここに書く
        }
}

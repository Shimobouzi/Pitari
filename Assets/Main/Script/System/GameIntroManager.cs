using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameIntroSequence : MonoBehaviour
{
    [Header("チュートリアルUI管理")]
    public TutorialUIManager tutorialUIManager; // チュートリアルUIを制御する共通のマネージャー

    [Header("チュートリアル画像セット")]
    public List<Sprite> firstTutorialSprites;   // 最初に表示するチュートリアル画像
    public List<Sprite> secondTutorialSprites;  // 2回目に表示するチュートリアル画像

    [Header("表示タイミング（秒）")]
    public float firstTutorialDelay = 1f;       // 最初のチュートリアル表示までの待機時間
    public float secondTutorialDelay = 11f;     // 2回目のチュートリアル表示までの待機時間（全体の時間）

    void Start()
    {
        // ゲーム開始時にチュートリアル表示シーケンスを開始
        StartCoroutine(ShowTutorialSequence());
    }

    /// <summary>
    /// チュートリアルを順番に表示するコルーチン
    /// </summary>
    IEnumerator ShowTutorialSequence()
    {
        // 1回目のチュートリアル表示まで待機
        yield return new WaitForSeconds(firstTutorialDelay);

        // 最初の画像セットを設定して表示
        tutorialUIManager.SetTutorialSprites(firstTutorialSprites);
        tutorialUIManager.ShowTutorial();

        // チュートリアルが閉じられるのを待つ
        bool tutorialClosed = false;
        TutorialUIManager.OnTutorialClosed += () => tutorialClosed = true;
        yield return new WaitUntil(() => tutorialClosed);

        // 2回目のチュートリアル表示までの残り時間を待機
        tutorialClosed = false;
        yield return new WaitForSeconds(secondTutorialDelay - firstTutorialDelay);

        // 2回目の画像セットを設定して表示
        tutorialUIManager.SetTutorialSprites(secondTutorialSprites);
        tutorialUIManager.ShowTutorial();

        // チュートリアルが閉じられるのを再度待つ
        yield return new WaitUntil(() => tutorialClosed);
    }
}

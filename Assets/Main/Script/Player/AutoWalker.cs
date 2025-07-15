using UnityEngine;

public class AutoWalker : MonoBehaviour
{
    [Header("移動設定")]
    public float speed = 2f;                  // プレイヤーの移動速度
    public Transform stopPoint;               // 移動の停止地点

    private Vector3 target;                   // 移動先の座標
    private bool walking = false;             // 移動中かどうかのフラグ
    private Animator animator;                // アニメーション制御用

    [Header("UI制御")]
    public TutorialUIManager tutorialUIManager; // チュートリアルUI表示用

    [Header("切り替え設定")]
    public GameObject replacementPrefab;      // 切り替え後に表示するプレハブ
    public float switchDelay = 14f;           // ゲーム開始から何秒後に切り替えるか

    [Header("切り替え後の位置・サイズ")]
    public Vector3 spawnOffset = Vector3.zero; // 切り替え後の位置オフセット
    public Vector3 newScale = Vector3.one;     // 切り替え後のサイズ

    [Header("足音設定")]
    public AudioClip footstepClip1;           // 足音1（例：Eki_player_concrete_ashioto1）
    public AudioClip footstepClip2;           // 足音2（例：Eki_player_concrete_ashioto2）
    public float footstepInterval = 0.5f;     // 足音の再生間隔（秒）

    private AudioSource audioSource;          // 足音再生用のAudioSource
    private bool useFirstClip = true;         // 足音の切り替えフラグ
    private float footstepTimer = 0f;         // 足音再生タイマー

    void Start()
    {
        // AnimatorとAudioSourceを取得・追加
        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>();

        // 移動先が設定されていれば移動開始
        if (stopPoint != null)
        {
            SetTarget(stopPoint.position);
        }

        // 指定秒数後にオブジェクト切り替え処理を実行
        Invoke("SwitchToReplacement", switchDelay);
    }

    // 移動先を設定して移動開始
    public void SetTarget(Vector3 position)
    {
        target = position;
        walking = true;

        if (animator != null)
        {
            animator.SetBool("isWalk", true);
        }
    }

    // 移動先に到達したかどうかを判定
    public bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, target) < 0.1f;
    }

    void Update()
    {
        if (walking)
        {
            // ターゲットに向かって移動
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            // 足音の再生タイミングを管理
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                PlayFootstep();
                footstepTimer = 0f;
            }

            // 到達したら停止処理
            if (HasReachedTarget())
            {
                walking = false;

                if (animator != null)
                {
                    animator.SetBool("isWalk", false);
                }

                if (tutorialUIManager != null)
                {
                    tutorialUIManager.ShowTutorial();
                }
            }
        }
    }

    // 足音を交互に再生する処理
    void PlayFootstep()
    {
        if (audioSource != null)
        {
            audioSource.clip = useFirstClip ? footstepClip1 : footstepClip2;
            audioSource.Play();
            useFirstClip = !useFirstClip; // 次回は別の足音に切り替える
        }
    }

    // 指定時間後にオブジェクトを切り替える処理
    void SwitchToReplacement()
    {
        if (replacementPrefab != null)
        {
            // 新しい位置と回転を設定
            Vector3 newPosition = transform.position + spawnOffset;
            Quaternion rotation = transform.rotation;

            // プレハブを生成
            GameObject newObject = Instantiate(replacementPrefab, newPosition, rotation);

            // 表示を有効化（非アクティブなプレハブでも表示されるように）
            newObject.SetActive(true);

            // サイズを変更
            newObject.transform.localScale = newScale;

            // 元のオブジェクトを削除
            Destroy(gameObject);
        }
    }
}

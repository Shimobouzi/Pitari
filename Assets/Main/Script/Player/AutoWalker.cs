using UnityEngine;

public class AutoWalker : MonoBehaviour
{
    public float speed = 2f;
    public Transform stopPoint;

    private Vector3 target;
    private bool walking = false;
    private Animator animator;

    public TutorialUIManager tutorialUIManager;

    [Header("切り替え設定")]
    public GameObject replacementPrefab; // 切り替え後のプレハブ
    public float switchDelay = 14f;      // ゲーム開始から何秒後に切り替えるか

    
    [Header("切り替え後の位置オフセット")]
    public Vector3 spawnOffset = Vector3.zero;

    [Header("切り替え後のサイズ")]
    public Vector3 newScale = Vector3.one; // 例：新しいサイズ（1,1,1 は元のサイズ）


    void Start()
    {
        animator = GetComponent<Animator>();

        if (stopPoint != null)
        {
            SetTarget(stopPoint.position);
        }

        // 指定秒数後に切り替え処理を実行
        Invoke("SwitchToReplacement", switchDelay);
    }

    public void SetTarget(Vector3 position)
    {
        target = position;
        walking = true;
        if (animator != null)
        {
            animator.SetBool("isWalk", true);
        }
    }

    public bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, target) < 0.1f;
    }

    void Update()
    {
        if (walking)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

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

    // GameObjectを切り替える処理

    void SwitchToReplacement()
    {
        if (replacementPrefab != null)
        {
            Vector3 newPosition = transform.position + spawnOffset;
            Quaternion rotation = transform.rotation;

            // プレハブを生成
            GameObject newObject = Instantiate(replacementPrefab, newPosition, rotation);

            // 表示を有効化
            newObject.SetActive(true);

            // サイズを変更
            newObject.transform.localScale = newScale;

            // 元のオブジェクトを削除
            Destroy(gameObject);
        }
    }



}

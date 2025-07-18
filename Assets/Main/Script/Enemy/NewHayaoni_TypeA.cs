using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class NewHayaoni_TypeA : MonoBehaviour
{
    [Header("移動・視界設定")]
    public float moveSpeed = 4f;
    public Vector2 visionSize = new Vector2(3f, 2f);
    public Vector2 visionOffset = new Vector2(-1.5f, 0f);
    public LayerMask playerLayer;

    [Header("足音設定")]
    public AudioClip footstep1;
    public AudioClip footstep2;
    public float footstepInterval = 0.5f;

    [Header("消える距離設定")]
    public float maxWalkDistance = 15f;

    private Transform player;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool hasDetectedPlayer = false;
    private bool useFirstFootstep = true;
    private float footstepTimer = 0f;
    private Vector2 startPosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("速鬼：プレイヤーが見つかりません");
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;

        BoxCollider2D col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        startPosition = transform.position;

        Debug.Log("速鬼：初期化完了");
    }

    private void Update()
    {
        if (hasDetectedPlayer || player == null) return;

        // 移動処理
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // 足音処理
        footstepTimer += Time.deltaTime;
        if (footstepTimer >= footstepInterval)
        {
            PlayFootstep();
            footstepTimer = 0f;
        }

        // 視界チェック
        Vector2 visionCenter = (Vector2)transform.position + visionOffset;
        Collider2D hit = Physics2D.OverlapBox(visionCenter, visionSize, 0f, playerLayer);

        if (hit != null && hit.CompareTag("Player"))
        {
            Debug.Log("速鬼：視界内にプレイヤーを検出");
            StartCoroutine(CheckPlayerHiding(hit.gameObject));
        }

        // 一定距離歩いたら削除
        float walkedDistance = Vector2.Distance(transform.position, startPosition);
        if (walkedDistance >= maxWalkDistance)
        {
            StopFootstepSound();
            Debug.Log("速鬼：一定距離を移動したため削除");
            Destroy(gameObject);
        }
    }

    private void PlayFootstep()
    {
        if (audioSource != null)
        {
            audioSource.clip = useFirstFootstep ? footstep1 : footstep2;
            audioSource.Play();
            useFirstFootstep = !useFirstFootstep;
        }
    }

    private void StopFootstepSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private IEnumerator CheckPlayerHiding(GameObject playerObj)
    {
        yield return new WaitForSeconds(0.35f); // 擬態待ち時間
        if (!IsPlayerHiding(playerObj))
        {
            hasDetectedPlayer = true;
            Debug.Log("速鬼：擬態していないプレイヤーを視界で発見 → ゲームオーバー");
            SceneManager.LoadScene("Result");
        }
        else
        {
            Debug.Log("速鬼：擬態中のプレイヤーを視界でスルー");
        }
    }

    private bool IsPlayerHiding(GameObject playerObj)
    {
        var joycon = playerObj.GetComponent<NewPlayerMove>();
        if (joycon != null) return joycon.GetisHiding();

        Debug.LogWarning("速鬼：プレイヤーの擬態状態を取得できませんでした");
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 visionCenter = (Vector2)transform.position + visionOffset;
        Gizmos.DrawWireCube(visionCenter, visionSize);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDetectedPlayer) return;

        if (other.CompareTag("Player"))
        {
            var playerMove = other.transform.parent.GetComponent<NewPlayerMove>();
            if (playerMove != null)
            {
                playerMove.OnBuruBuru();
                Debug.Log("速鬼：プレイヤーと接触しました！");

                if (!playerMove.GetisHiding())
                {
                    hasDetectedPlayer = true;
                    Debug.Log("速鬼：擬態していないプレイヤーと接触 → ゲームオーバー");
                    SceneManager.LoadScene("Result");
                }
                else
                {
                    Debug.Log("速鬼：擬態中のプレイヤーと接触 → スルー");
                }
            }
        }
    }
}

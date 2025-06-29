using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Hayaoni_3D : MonoBehaviour
{
    public Transform player;           // プレイヤーの Transform
    public float moveSpeed = 3f;       // 敵の移動速度
    public float smoothTime = 0.2f;    // 追跡の滑らかさ

    private Rigidbody rb;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position;
            Vector3 newPosition = Vector3.SmoothDamp(rb.position, targetPosition, ref velocity, smoothTime, moveSpeed);
            rb.MovePosition(newPosition);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("【敵】プレイヤーに接触しました！ → ゲームオーバー処理を実行します。");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

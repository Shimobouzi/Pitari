using UnityEngine;

public class ChasingOni : MonoBehaviour
{
    [Header("移動設定")]
    [Tooltip("プレイヤーを追いかける速度")]
    public float moveSpeed = 4f;

    public Transform player;

    void Update()
    {
        ChasePlayer();
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("速鬼がプレイヤーに接触！ゲームオーバー！");
            GameOver();
        }
    }

    void GameOver()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}

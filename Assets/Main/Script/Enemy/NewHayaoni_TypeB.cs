using UnityEngine;
using UnityEngine.SceneManagement;

public class NewHayaoni_TypeB : MonoBehaviour
{
    public Transform player;      // プレイヤーの位置
    public float speed = 6f;      // 速鬼の移動速度

    void Update()
    {
        if (player != null)
        {
            // プレイヤーの方向を計算して移動
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("速鬼：プレイヤーと接触しました！ → ゲームオーバー");

            // プレイヤーの Joy-Con を振動させる
            var playerMove = other.transform.parent?.GetComponent<NewPlayerMove>();
            if (playerMove != null)
            {
                playerMove.OnBuruBuru(); // 振動処理を呼び出す
            }

            // ゲームオーバー画面に遷移
            SceneManager.LoadScene("Result");
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySightChase : MonoBehaviour
{
    public float moveSpeed = 3f;

    private void Update()
    {
        // 敵は常に右→左へ移動
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーが擬態していなければゲームオーバー
            if (!PlayerMove.GetisHiding())
            {
                Debug.Log("視界内で擬態してない → ゲームオーバー");
                GameOver();
            }
            else
            {
                Debug.Log("視界内で擬態中 → スルー");
            }
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

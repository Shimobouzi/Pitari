using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

/// <summary>
/// プレイヤーを追いかける速鬼（接触でゲームオーバー）
/// </summary>
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

    /// <summary>
    /// プレイヤーに接触したらログを出してゲームオーバー画面へ遷移
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("速鬼：プレイヤーと接触しました！");
            Debug.Log("速鬼がプレイヤーに接触しました！ → ゲームオーバー");

            // ゲームオーバー画面に遷移
            SceneManager.LoadScene("Result");
        }
    }
}

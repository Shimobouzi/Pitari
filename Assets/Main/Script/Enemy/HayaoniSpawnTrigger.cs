using UnityEngine;

/// <summary>
/// プレイヤーが2Dトリガーに触れたら速鬼（NewHayaoni_TypeB）を出現させるスクリプト
/// </summary>
public class HayaoniSpawnTrigger : MonoBehaviour
{
    // 出現させる速鬼のプレハブ（Projectビューに保存されたもの）
    public GameObject hayaoniPrefab;

    // 速鬼の出現位置（空の GameObject を使う）
    public Transform spawnPoint;

    // プレイヤーの Transform（速鬼が追いかける対象）
    public Transform player;

    // 一度だけ出現させるためのフラグ
    private bool hasSpawned = false;

    /// <summary>
    /// スクリプトが開始されたときにログを出す（確認用）
    /// </summary>
    void Start()
    {
        //Debug.Log("HayaoniSpawnTrigger スクリプトが開始されました。");
    }

    /// <summary>
    /// 毎フレームログを出すことで、スクリプトが動いているか確認できる
    /// </summary>
    void Update()
    {
        //Debug.Log("HayaoniSpawnTrigger スクリプトは動いています。");
    }

    /// <summary>
    /// プレイヤーが2Dトリガーに入ったときに速鬼を出現させる
    /// </summary>
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("2Dトリガーに何かが入りました: " + other.name);

        // プレイヤーがトリガーに入ったかつ、まだ出現していない場合
        if (!hasSpawned && other.CompareTag("Player"))
        {
            Debug.Log("プレイヤーが2Dトリガーに入りました。速鬼を出現させます。");

            // 必要な参照が設定されているか確認
            if (hayaoniPrefab == null || spawnPoint == null || player == null)
            {
                Debug.LogError("必要な参照が設定されていません！（プレハブ、出現位置、プレイヤー）");
                return;
            }

            // プレハブを出現位置に生成
            GameObject oni = Instantiate(hayaoniPrefab, spawnPoint.position, Quaternion.identity);
            Debug.Log("速鬼を出現させました: " + oni.name);

            // プレイヤーを速鬼に渡す（追いかけ対象を設定）
            NewHayaoni_TypeB oniScript = oni.GetComponent<NewHayaoni_TypeB>();
            if (oniScript != null)
            {
                oniScript.player = player;
                Debug.Log("速鬼にプレイヤーを設定しました。");
            }
            else
            {
                Debug.LogWarning("NewHayaoni_TypeB スクリプトがプレハブに見つかりません！");
            }

            // 一度だけ出現させるようにフラグを更新
            hasSpawned = true;
        }
    }
}

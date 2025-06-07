using Unity.VisualScripting;
using UnityEngine;

public class SpownEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _player;
    private Vector3 spownPosi;
    private void Start()
    {
        this.transform.position = new Vector3(this.transform.position.x, _player.transform.GetChild(0).position.y , this.transform.position.z);
        spownPosi = new Vector3(transform.position.x, _player.transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        SpownEnemyF(_enemyPrefab);
        Destroy(this.gameObject);
    }


    private void SpownEnemyF(GameObject enemy)
    {
        Instantiate(enemy, spownPosi, Quaternion.identity);
    }
}

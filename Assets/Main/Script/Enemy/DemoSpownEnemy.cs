using Unity.VisualScripting;
using UnityEngine;

public class DemoSpownEnemy : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    private Vector3 spownPosi;
    private void Start()
    {
        spownPosi = new Vector3(transform.position.x, -2.09f, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SpownEnemy(_enemyPrefab);
        Destroy(this.gameObject);
    }


    private void SpownEnemy(GameObject enemy)
    {
        Instantiate(enemy, spownPosi, Quaternion.identity);
    }
}

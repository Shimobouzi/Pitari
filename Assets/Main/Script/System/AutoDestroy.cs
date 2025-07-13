using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float lifetime = 1.0f; // 寿命（秒）

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField, Header("移動速度")]//unity上で値の変更が可能
    private float _moveSpeed;//敵の移動速度変数

    private Rigidbody2D _rigid;

    //Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    //Update is called once per frame
    void Update()
    {
        _Move();
    }

    private void _Move()
    {
        _rigid.linearVelocity = new Vector2(Vector2.left.x * _moveSpeed, _rigid.linearVelocity.y);
    }
}

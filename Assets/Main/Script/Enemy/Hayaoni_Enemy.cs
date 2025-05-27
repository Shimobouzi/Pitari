using UnityEngine;

public class Hayaoni_Enemy : EnemyBase
{
    protected override void Start()
    {
        nomalSpeed *= 3f;
        base.Start();
    }


    override protected void Idel()
    {
        StartCoroutine(MoveLeft(50f, nomalSpeed));
    }
}

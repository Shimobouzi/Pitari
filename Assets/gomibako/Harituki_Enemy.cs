using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Harituki_Enemy : EnemyBase
{
    protected override void Start()
    {
        coolTime = 5f;
        nomalSpeed = 0;
        runSpeed = 0;
        base.Start();
    }

    protected override void Idel()
    {
        StartCoroutine(Gyorori(coolTime));
    }

    private IEnumerator Gyorori(float duration)
    {
        this.transform.Find("EnemyView").gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        this.transform.Find("EnemyView").gameObject.SetActive(false);
        yield return new WaitForSeconds(duration);
        StartCoroutine(Gyorori(duration));
    }
}

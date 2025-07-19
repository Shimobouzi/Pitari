using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BAKE : MonoBehaviour
{
    public Animator animator;
    private bool hasDetectedPlayer = false;
    private void Start()
    {
        StartCoroutine(HIRAKU());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasDetectedPlayer) return;

        if (other.CompareTag("Player"))
        {
            var playerMove = other.transform.parent.GetComponent<NewPlayerMove>();
            if (playerMove != null)
            {
                //playerMove.OnBuruBuru();
                Debug.Log("nelo：プレイヤーと接触しました！");

                if (!playerMove.GetisHiding())
                {
                    hasDetectedPlayer = true;
                    Debug.Log("neko：擬態していないプレイヤーと接触 → ゲームオーバー");
                    SceneManager.LoadScene("Result");
                }
                else
                {
                    Debug.Log("neko：擬態中のプレイヤーと接触 → スルー");
                }
            }
        }
    }
    IEnumerator HIRAKU()
    {
        animator.SetBool("Bake", true);
        yield return new WaitForSeconds(3f);
        animator.SetBool("Bake", false);
        yield return new WaitForSeconds(3f);
        StartCoroutine(HIRAKU());
    }
}

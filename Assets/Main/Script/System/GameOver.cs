using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public Animator animator;
    public static GameOver instance;
    public bool BOOL = false;
    private void Awake()
    {
        instance = this;
        this.gameObject.SetActive(false);
    }

    public void OnGameOver()
    {
        BOOL = true;
        this.gameObject.SetActive(true);
        AudioManager.PlaySE("GameO");
        animator.SetBool("Over", true);
        StartCoroutine(resetBool());
    }

    IEnumerator resetBool()
    {
        
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
        animator.SetBool("Over", false);
        BOOL = false;
    }
}

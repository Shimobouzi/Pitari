using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class Hayaoni_Haritsuki : MonoBehaviour
{
    public float eyeOpenDuration = 2f;   // 目が開いている時間
    public float eyeClosedDuration = 2f; // 目が閉じている時間

    private bool isEyeOpen = false;
    private float timer = 0f;
    private float currentDuration;

    private Transform player;
    private Animator animator;
    private BoxCollider2D col;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;

        currentDuration = eyeClosedDuration;
        UpdateEyeState(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= currentDuration)
        {
            timer = 0f;
            isEyeOpen = !isEyeOpen;
            currentDuration = isEyeOpen ? eyeOpenDuration : eyeClosedDuration;
            UpdateEyeState(isEyeOpen);
        }

        if (isEyeOpen)
        {
            CheckPlayer();
        }
    }

    private void UpdateEyeState(bool open)
    {
        if (animator != null)
        {
            animator.SetBool("isEyeOpen", open);
        }
    }

    private void CheckPlayer()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, col.size, 0f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player") && !IsPlayerHiding())
            {
                Debug.Log("張り付き男：擬態していないプレイヤーを発見 → ゲームオーバー");
                GameOver();
            }
        }
    }

    private bool IsPlayerHiding()
    {
        var joycon = player.GetComponent<PlayerMove>();
        if (joycon != null) return PlayerMove.GetisHiding();

        var ps4 = player.GetComponent<PlayerMove_PS4>();
        if (ps4 != null) return PlayerMove_PS4.GetIsHiding();

        return false;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isEyeOpen ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().size);
    }
}

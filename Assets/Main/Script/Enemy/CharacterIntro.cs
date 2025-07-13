using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 middlePos;
    private Vector3 exitPos;

    private int phase = 0;
    private float timer = 0f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        startPos = new Vector3(32f, -4f, 0f);
        middlePos = new Vector3(-4f, -4f, 0f);
        exitPos = new Vector3(-20f, -4f, 0f);

        transform.position = startPos;
        sr.flipX = false;

        animator.SetBool("IsWalking", true);
    }

    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        switch (phase)
        {
            case 0:
                transform.position = Vector3.MoveTowards(transform.position, middlePos, moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, middlePos) < 0.01f)
                {
                    animator.SetBool("IsWalking", false); // 止まる
                    timer = 0f;
                    phase = 1;
                }
                break;

            case 1:
                timer += Time.deltaTime;
                if (state.IsName("Idle") && timer > 0.2f)
                {
                    animator.SetTrigger("StartLookAround");
                    timer = 0f;
                    phase = 2;
                }
                break;

            case 2:
                timer += Time.deltaTime;
                if (state.IsName("look around") && state.normalizedTime >= 1f)
                {
                    animator.SetBool("IsWalking", true); // → walk に戻る
                    phase = 3;
                }
                break;

            case 3:
                transform.position = Vector3.MoveTowards(transform.position, exitPos, moveSpeed * Time.deltaTime);
                break;
        }
    }
}

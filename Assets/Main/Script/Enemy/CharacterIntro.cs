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

    [Header("足音設定")]
    public AudioClip footstepClip1;
    public AudioClip footstepClip2;
    public float footstepInterval = 0.5f;
    public float footstepStartDelay = 17f; // 足音開始までの待機時間

    private AudioSource audioSource;
    private bool useFirstClip = true;
    private float footstepTimer = 0f;
    private bool canPlayFootsteps = false;

    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        startPos = new Vector3(38f, -4f, -4f);
        middlePos = new Vector3(-4f, -4f, -4f);
        exitPos = new Vector3(-20f, -4f, -4f);

        transform.position = startPos;
        sr.flipX = false;

        animator.SetBool("IsWalking", true);

        audioSource = gameObject.AddComponent<AudioSource>();

        // 17秒後に足音再生を許可
        Invoke("EnableFootsteps", footstepStartDelay);
    }

    void EnableFootsteps()
    {
        canPlayFootsteps = true;
    }

    void Update()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        switch (phase)
        {
            case 0:
                transform.position = Vector3.MoveTowards(transform.position, middlePos, moveSpeed * Time.deltaTime);
                HandleFootsteps(state);
                if (Vector3.Distance(transform.position, middlePos) < 0.01f)
                {
                    animator.SetBool("IsWalking", false);
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
                    animator.SetBool("IsWalking", true);
                    phase = 3;
                }
                break;

            case 3:
                transform.position = Vector3.MoveTowards(transform.position, exitPos, moveSpeed * Time.deltaTime);
                HandleFootsteps(state);
                break;
        }
    }

    void HandleFootsteps(AnimatorStateInfo state)
    {
        if (canPlayFootsteps && state.IsName("walk"))
        {
            footstepTimer += Time.deltaTime;
            if (footstepTimer >= footstepInterval)
            {
                audioSource.clip = useFirstClip ? footstepClip1 : footstepClip2;
                audioSource.Play();
                useFirstClip = !useFirstClip;
                footstepTimer = 0f;
            }
        }
    }
}

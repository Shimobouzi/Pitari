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
    public float footstepStartDelay = 17f;

    private AudioSource audioSource;
    private bool useFirstClip = true;
    private float footstepTimer = 0f;
    private bool canPlayFootsteps = false;

    [Header("非表示・音声停止設定")]
    public float hideDelay = 32f; // 指定秒数後に非表示・停止

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

        // 足音再生許可（17秒後）
        Invoke("EnableFootsteps", footstepStartDelay);

        // 指定秒数後に音声停止と非表示処理
        Invoke("StopAudioAndHide", hideDelay);
    }

    void EnableFootsteps()
    {
        canPlayFootsteps = true;
    }

    void StopAudioAndHide()
    {
        // 足音停止
        canPlayFootsteps = false;

        // 音声停止
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // アニメーション停止
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }

        // オブジェクト非表示
        gameObject.SetActive(false);

        // スクリプト停止
        enabled = false;
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

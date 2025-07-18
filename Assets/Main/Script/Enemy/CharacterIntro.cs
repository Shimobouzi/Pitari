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
    public AudioClip footstepClip;
    public float footstepInterval = 0.5f;
    public float footstepStartDelay = 17f;

    [Header("座敷童設定")]
    public bool isZashikiWarashi = false;
    public float zashikiFootstepInterval = 1.2f;

    [Header("追加効果音")]
    public AudioClip startFootstepSound;
    public AudioClip lookAroundSound;

    [Header("音量設定（1.0以上で増幅可能）")]
    public float footstepVolume = 1f;
    public float startFootstepVolume = 1f;
    public float lookAroundVolume = 1f;

    [Header("見回す効果音の遅延設定")]
    public float lookAroundSoundDelay = 0.5f;

    [Header("非表示・音声停止設定")]
    public float hideDelay = 32f;

    private AudioSource audioSource;
    private float footstepTimer = 0f;
    private bool canPlayFootsteps = false;
    private bool hasPlayedLookSound = false;

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
        audioSource.playOnAwake = false;

        Invoke("EnableFootsteps", footstepStartDelay);
        Invoke("StopAudioAndHide", hideDelay);
    }

    void EnableFootsteps()
    {
        canPlayFootsteps = true;

        if (startFootstepSound != null)
        {
            audioSource.PlayOneShot(startFootstepSound, startFootstepVolume);
        }
    }

    void StopAudioAndHide()
    {
        canPlayFootsteps = false;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }

        gameObject.SetActive(false);
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
                if (state.IsName("look around"))
                {
                    if (timer >= lookAroundSoundDelay && !hasPlayedLookSound && lookAroundSound != null)
                    {
                        audioSource.PlayOneShot(lookAroundSound, lookAroundVolume);
                        hasPlayedLookSound = true;
                    }

                    if (state.normalizedTime >= 1f)
                    {
                        animator.SetBool("IsWalking", true);
                        phase = 3;
                    }
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
            float currentInterval = isZashikiWarashi ? zashikiFootstepInterval : footstepInterval;

            footstepTimer += Time.deltaTime;
            if (footstepTimer >= currentInterval)
            {
                audioSource.volume = Mathf.Clamp(footstepVolume, 0f, 10f);
                audioSource.clip = footstepClip;
                audioSource.Play();
                footstepTimer = 0f;
            }
        }
    }
}

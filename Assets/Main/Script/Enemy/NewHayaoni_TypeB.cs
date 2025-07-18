using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ← IEnumerator を使うために必要！

public class NewHayaoni_TypeB : MonoBehaviour
{
    public Transform player;
    public float speed = 6f;

    [Header("足音設定")]
    public AudioClip footstep1;
    public AudioClip footstep2;
    public float footstepInterval = 0.5f;
    public float footstepVolume = 1f;
    public float footstepDuration = 8f; // 足音を鳴らす秒数

    private AudioSource audioSource;
    private float footstepTimer = 0f;
    private bool playFirstFootstep = true;
    private bool canPlayFootsteps = true;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        StartCoroutine(StopFootstepsAfterDelay(footstepDuration));
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (canPlayFootsteps)
            {
                footstepTimer += Time.deltaTime;
                if (footstepTimer >= footstepInterval)
                {
                    PlayFootstep();
                    footstepTimer = 0f;
                }
            }
        }
    }

    void PlayFootstep()
    {
        if (playFirstFootstep && footstep1 != null)
        {
            audioSource.PlayOneShot(footstep1, footstepVolume);
        }
        else if (!playFirstFootstep && footstep2 != null)
        {
            audioSource.PlayOneShot(footstep2, footstepVolume);
        }

        playFirstFootstep = !playFirstFootstep;
    }

    IEnumerator StopFootstepsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canPlayFootsteps = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("速鬼：プレイヤーと接触しました！ → ゲームオーバー");

            var playerMove = other.transform.parent?.GetComponent<NewPlayerMove>();
            if (playerMove != null)
            {
                // playerMove.OnBuruBuru(); // 振動処理
            }

            SceneManager.LoadScene("Result");
        }

        if (other.CompareTag("VideoTriggerZone"))
        {
            Debug.Log("速鬼：VideoTriggerZone に触れたので消えます");
            gameObject.SetActive(false);
        }
    }
}

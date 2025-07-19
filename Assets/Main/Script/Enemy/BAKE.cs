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
                Debug.Log("nelo�F�v���C���[�ƐڐG���܂����I");

                if (!playerMove.GetisHiding())
                {
                    hasDetectedPlayer = true;
                    Debug.Log("neko�F�[�Ԃ��Ă��Ȃ��v���C���[�ƐڐG �� �Q�[���I�[�o�[");
                    SceneManager.LoadScene("Result");
                }
                else
                {
                    Debug.Log("neko�F�[�Ԓ��̃v���C���[�ƐڐG �� �X���[");
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

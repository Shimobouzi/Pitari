
using UnityEngine;

public class AutoWalker : MonoBehaviour
{
    public float speed = 2f;
    public Transform stopPoint;

    private Vector3 target;
    private bool walking = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (stopPoint != null)
        {
            SetTarget(stopPoint.position);
        }
    }

    public void SetTarget(Vector3 position)
    {
        target = position;
        walking = true;
        if (animator != null)
        {
            animator.SetBool("isWalk", true);
        }
    }

    public bool HasReachedTarget()
    {
        return Vector3.Distance(transform.position, target) < 0.1f;
    }

    void Update()
    {
        if (walking)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            if (HasReachedTarget())
            {
                walking = false;
                if (animator != null)
                {
                    animator.SetBool("isWalk", false);
                }
            }
        }
    }
}

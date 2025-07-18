using UnityEngine;

public class AutoMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    private bool isMoving = true;

    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    public void StopMovement()
    {
        isMoving = false;
    }
}

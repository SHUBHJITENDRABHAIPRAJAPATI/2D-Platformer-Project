using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 3f;
    public float distance = 0.5f;

    private Vector3 startPos;
    private bool movingRight = true;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, startPos) >= distance)
                movingRight = false;
        }
        else
        {
            transform.position -= Vector3.right * speed * Time.deltaTime;
            if (Vector3.Distance(transform.position, startPos) <= 0.1f)
                movingRight = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}

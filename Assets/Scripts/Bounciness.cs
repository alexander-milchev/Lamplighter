using UnityEngine;

public class Bounciness : MonoBehaviour
{
    [SerializeField] private float forceSize = 10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocityY = 0f;
            rb.AddForce(Vector2.up*forceSize, ForceMode2D.Impulse);
        }
    }
}

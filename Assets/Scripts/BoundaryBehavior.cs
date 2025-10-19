using UnityEngine;

public class BoundaryBehavior : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("BarrelContent")) Destroy(collision.gameObject);
    }
}

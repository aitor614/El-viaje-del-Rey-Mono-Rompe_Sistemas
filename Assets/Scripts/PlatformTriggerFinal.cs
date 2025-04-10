using UnityEngine;

public class PlatformTriggerFinal : MonoBehaviour
{
    public GameObject finalPlatform;
    public float offsetY = 2f;
    private bool triggered = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!triggered && col.collider.CompareTag("Player"))
        {
            triggered = true;

            // Limita el spawn a la zona visible
            float leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
            float rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;

            float halfWidth = finalPlatform.GetComponent<SpriteRenderer>().bounds.extents.x;
            float safeLeft = leftEdge + halfWidth;
            float safeRight = rightEdge - halfWidth;

            float randomX = Random.Range(safeLeft, safeRight);
            float finalY = transform.position.y + offsetY;

            Vector3 spawnPos = new Vector3(randomX, finalY, 0f);
            Instantiate(finalPlatform, spawnPos, Quaternion.identity);
        }
    }
}

using UnityEngine;

public class DestroyBelowCamera : MonoBehaviour
{
    public float offsetY = 6f;

    void Update()
    {
        if (transform.position.y < Camera.main.transform.position.y - offsetY)
        {
            Destroy(gameObject);
        }
    }
}

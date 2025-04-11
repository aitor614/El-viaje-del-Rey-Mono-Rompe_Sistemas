using UnityEngine;

public class Brick : MonoBehaviour
{
    public AudioClip destroySound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            AudioSource.PlayClipAtPoint(destroySound, transform.position);
            Destroy(gameObject);
           
        }


    }
  



}

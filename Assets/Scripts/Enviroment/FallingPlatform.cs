using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;
    private bool falling = false;
    [SerializeField] private Rigidbody2D rb;

    private Coroutine fallCoroutine; // coroutine reference
    private Vector3 originalPosition; // original position

    private void Start() {
        rb.bodyType = RigidbodyType2D.Kinematic; // makes sure it is kimematic
        originalPosition = transform.position; // saves the original position
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (falling) return;
        if (collision.transform.CompareTag("Player")) {
            StartFall();
        }
    }

    public void StartFall() {
        fallCoroutine = StartCoroutine(StartFallCoroutine());
    }

    private IEnumerator StartFallCoroutine() {
        falling = true;
        yield return new WaitForSeconds(fallDelay);
        AudioManager.instance.Play("Falling");
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(destroyDelay);
    }

    public void ResetPlatform() { //resets platform to original positon

        if (fallCoroutine != null) {
            StopCoroutine(fallCoroutine);
        }

        falling = false;
        rb.bodyType = RigidbodyType2D.Kinematic; // reset to Kinematic
        transform.position = originalPosition; // reset to original position
        rb.velocity = Vector2.zero;
    }
}




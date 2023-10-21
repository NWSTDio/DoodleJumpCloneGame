using UnityEngine;

public class PlatformMovedScript : MonoBehaviour {

    public enum DIRECTION { HORIZONTAL, VERTICAL, SMALL_HORIZONTAL }

    [SerializeField] DIRECTION dir;
    [SerializeField] float speed = 1, min = .5f, max = .5f;

    private float startPos;
    private bool forward;
    private Rigidbody2D rb;

    private void Start() {
        if (dir == DIRECTION.HORIZONTAL || dir == DIRECTION.SMALL_HORIZONTAL)
            startPos = transform.position.x;
        else if (dir == DIRECTION.VERTICAL)
            startPos = transform.position.y;

        forward = Random.Range(0, 2) == 0;

        rb = GetComponent<Rigidbody2D>();
        }
    private void FixedUpdate() {
        if (dir == DIRECTION.HORIZONTAL) {
            if (transform.position.x > 2 * .8f)
                forward = false;
            else if (transform.position.x < -2 * .8f)
                forward = true;

            rb.velocity = (forward ? Vector2.right : Vector2.left) * speed;
            }
        else if (dir == DIRECTION.SMALL_HORIZONTAL) {
            if (transform.position.x > startPos + max)
                forward = false;
            else if (transform.position.x < startPos - min)
                forward = true;

            rb.velocity = (forward ? Vector2.right : Vector2.left) * speed;
            }
        else if (dir == DIRECTION.VERTICAL) {
            if (transform.position.y > startPos + max)
                forward = false;
            else if (transform.position.y < startPos - min)
                forward = true;

            rb.velocity = (forward ? Vector2.up : Vector2.down) * speed;
            }
        }

    public void ChangeDirection(DIRECTION direction) => dir = direction;

    }
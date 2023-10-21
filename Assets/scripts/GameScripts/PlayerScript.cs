using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]

public class PlayerScript : MonoBehaviour {
    [SerializeField] GameController controller;
    Rigidbody2D rb;

    [SerializeField] float jumpSpeed = 10, moveSpeed = 3, springSpeed = 12, flySpeed = 2, bonusSpringSpeed = 14;
    [SerializeField] LayerMask platformLayer;
    [SerializeField] float radius;
    [SerializeField] Transform checkPosition, firePosition, balls, cam;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject Shield, Helmet, Spring;

    SpriteRenderer sprite;
    bool jump = false, isSpring = false, fire = false;

    [SerializeField] private AudioSource JumpSound, SpringSound, ShootSound, ItemSound, BonusJumpSound;

    bool bonusShield = false, bonusFly = false, bonusSpring;
    float bonusTimer = 0f;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        jumpSpeed = (!Application.isMobilePlatform) ? jumpSpeed : jumpSpeed + 1;
        moveSpeed = (!Application.isMobilePlatform) ? moveSpeed : moveSpeed * 2;
        springSpeed = (!Application.isMobilePlatform) ? springSpeed : springSpeed + 1;
        bonusSpringSpeed = (!Application.isMobilePlatform) ? bonusSpringSpeed : bonusSpringSpeed + 1;
        }
    void FixedUpdate() {
        if (Data.RUNNING) {
            if (!controller.FREEZE) {
                if (bonusFly && cam.position.y < transform.position.y)
                    controller.MoveCamera(transform.position.y, true);
                else {
                    Collider2D collider = null;
                    if (!isSpring)
                        collider = Physics2D.OverlapCircle(checkPosition.position, radius, platformLayer);
                    if (collider != null) {
                        if (cam.position.y < collider.transform.position.y)
                            controller.MoveCamera(collider.transform.position.y);
                        if (collider.gameObject.tag.Equals("Spring") && rb.velocity.y <= 0)
                            isSpring = true;
                        if (collider.gameObject.tag.Equals("Platform") && rb.velocity.y <= 0)
                            jump = true;
                        if (collider.gameObject.tag.Equals("SpawnerPlatform") && rb.velocity.y <= 0)
                            jump = true;
                        if (collider.gameObject.tag.Equals("Monster") && rb.velocity.y <= 0)
                            jump = true;
                        if (collider.gameObject.tag.Equals("MovedPlatform") && rb.velocity.y <= 1)
                            jump = true;
                        if (collider.gameObject.tag.Equals("MovedSpringPlatform") && rb.velocity.y <= 1)
                            isSpring = true;
                        if (collider.gameObject.tag.Equals("DestroyPlatform") && rb.velocity.y <= 0)
                            collider.GetComponent<PlatformScript>().Destroyer();
                        if (collider.gameObject.tag.Equals("HidePlatform") && rb.velocity.y <= 0) {
                            jump = true;
                            Destroy(collider.gameObject);
                            }
                        }
                    }

                float move = Input.GetAxis("Horizontal");
                if (Application.isMobilePlatform) {
                    Vector3 accel = controller.FixAccel(Input.acceleration);
                    if (Mathf.Abs(accel.x) > .1f)
                        move = accel.x;
                    }

                if (move < 0 && sprite.flipX)
                    sprite.flipX = false;
                else if (move > 0 && !sprite.flipX)
                    sprite.flipX = true;

                rb.velocity = new Vector2(move * moveSpeed, rb.velocity.y);
                }
            }
        }
    void Update() {
        if (Data.RUNNING) {
            rb.bodyType = RigidbodyType2D.Dynamic;
            if (!controller.FREEZE) {
                if (bonusFly)
                    rb.velocity = Vector2.up * flySpeed;
                else {
                    if (isSpring) {
                        isSpring = false;
                        if (Data.SOUND)
                            ((bonusSpring) ? BonusJumpSound : JumpSound).Play();
                        rb.velocity = Vector2.up * ((bonusSpring) ? bonusSpringSpeed : springSpeed);
                        }
                    if (jump) {
                        jump = false;
                        if (Data.SOUND)
                            ((bonusSpring) ? BonusJumpSound : JumpSound).Play();
                        rb.velocity = Vector2.up * ((bonusSpring) ? bonusSpringSpeed : jumpSpeed);
                        }
                    }

                if (transform.position.y < cam.position.y - 14.0f)
                    controller.GameOver();
                if (transform.position.x < -2)
                    transform.position = new Vector2(2, transform.position.y);
                else if (transform.position.x > 2)
                    transform.position = new Vector2(-2, transform.position.y);
                }
            if (Input.GetKeyDown(KeyCode.Q))
                Fire();
            if (bonusTimer > 0) {
                bonusTimer -= Time.deltaTime;
                if (bonusTimer <= 0) {
                    if (bonusShield) {
                        bonusShield = false;
                        Shield.SetActive(false);
                        }
                    else if (bonusFly) {
                        bonusFly = false;
                        rb.gravityScale = 1;
                        GetComponent<CapsuleCollider2D>().isTrigger = false;
                        Helmet.SetActive(false);
                        }
                    else if (bonusSpring) {
                        bonusSpring = false;
                        Spring.SetActive(false);
                        }
                    }
                }
            if (transform.position.y > controller.GetScope())
                controller.changeScopeText(transform.position.y);
            }
        else
            rb.bodyType = RigidbodyType2D.Static;
        }
    void OnCollisionEnter2D(Collision2D collision) {
        if (!bonusShield && collision.gameObject.tag.Equals("Monster") && !bonusFly) {
            if (transform.position.y < collision.transform.position.y)
                controller.GameOver();
            }
        }
    void OnTriggerEnter2D(Collider2D collision) {
        if (!bonusFly) {
            if (collision.gameObject.tag.Equals("ShieldItem")) {
                if (Data.SOUND)
                    ItemSound.Play();
                bonusShield = true;
                Shield.SetActive(true);
                bonusTimer = 10f;
                Destroy(collision.gameObject);
                }
            else if (collision.gameObject.tag.Equals("HelmetItem")) {
                if (Data.SOUND)
                    ItemSound.Play();
                bonusFly = true;
                rb.gravityScale = 0;
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                Helmet.SetActive(true);
                bonusTimer = 10f;
                Destroy(collision.gameObject);
                }
            else if (collision.gameObject.tag.Equals("SpringItem")) {
                if (Data.SOUND)
                    ItemSound.Play();
                bonusSpring = true;
                Spring.SetActive(true);
                bonusTimer = 10f;
                Destroy(collision.gameObject);
                }
            }
        }
    public void Fire() {
        if (Data.RUNNING && !fire && !bonusFly) {
            fire = true;
            Vector3 difference = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z) - transform.position;
            float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 2f);
            for (int i = 0; i < collider.Length; i++) {
                if (collider[i].gameObject.tag.Equals("Monster")) {
                    difference = collider[i].transform.position - transform.position;
                    rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                    break;
                    }
                }
            if (Data.SOUND)
                ShootSound.Play();
            _fire(firePosition.position, Quaternion.Euler(0f, 0f, rotateZ));
            sprite.sprite = sprites[1];
            StartCoroutine(_changeSprite());
            }
        }
    IEnumerator _changeSprite() {
        yield return new WaitForSeconds(.5f);
        fire = false;
        sprite.sprite = sprites[0];
        }
    void _fire(Vector3 position, Quaternion quaternion) {
        GameObject ball = Instantiate(ballPrefab, position, quaternion) as GameObject;
        ball.transform.SetParent(balls);
        }
    }
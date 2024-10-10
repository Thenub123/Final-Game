using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Movement : MonoBehaviour {


    [Header("Movement Variables")]
    public float baseSpeed = 2.8f;
    private float runSpeed = 0f;
    public float baseJumpForce = 2.6f;
    private float jumpForce = 2.6f;

    public float coyoteTiming = 0;
    private float coyoteTimer = 0;

    public float groundCheckRadius;

    private int horizontalPressed = 0;
    private int lastHorizontalPressed = 0;

    public bool isDead = false;
    public float baseDeathTimer = 200;

    public bool isFrozen = false;

    [Header("References")]
    public Sprite jumpSprite;

    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator animator;
    
    public GameObject spriteObj;

    private bool isGrounded;
    public GameObject groundCheckPoint;
    
    public LayerMask groundLayer;

    private bool jumpPressed = false;
    private bool canJump = true;
    private float y_velocity = 0;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        sr = spriteObj.GetComponent<SpriteRenderer>();
        animator = spriteObj.GetComponent<Animator>();
    }

    void Update() {

        Anim();

        if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;
        if (Input.GetKeyUp(KeyCode.Space)) jumpPressed = false;
        
        if (Input.GetKey(KeyCode.A)) horizontalPressed = -1;
        if (Input.GetKey(KeyCode.D)) horizontalPressed = 1;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) horizontalPressed = 0;
        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) horizontalPressed = 0;

        if (horizontalPressed != 0) {
            animator.SetBool("Running", true);
        }
        else{
            animator.SetBool("Running", false);
        }

        y_velocity = body.velocity.y;
    }

    void FixedUpdate() {
        if (!isDead) {
            Move();
            Jump();
        }
    }

    void Move() {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius, groundLayer);
        if (horizontalPressed != 0) {
            if (runSpeed < baseSpeed) {
                runSpeed += 0.5f;
            }
            lastHorizontalPressed = horizontalPressed;
            body.velocity = new Vector2(horizontalPressed * runSpeed, body.velocity.y);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, (horizontalPressed * -90) + 90, transform.eulerAngles.z);
        }
        else
        {
            animator.SetBool("Running", false);
            if (runSpeed > 0.1f) {
                runSpeed -= 0.4f;
            }
            if (runSpeed < 0.2f) {
                runSpeed = 0;
            }
            body.velocity = new Vector2(lastHorizontalPressed * runSpeed, body.velocity.y);
        }
    }

    void Anim() {
        if (!isDead) {
            animator.SetBool("isDead", false);
            if (body.velocity.y > 0.1f && !isGrounded) {
                animator.SetInteger("Jump", 1);
            }
            if (body.velocity.y < -0.1f && !isGrounded) {
                animator.SetInteger("Jump", 2);
            }
            if (isGrounded) {
                animator.SetInteger("Jump", 0);
            }
            if (!isGrounded && animator.GetInteger("Jump") != 1) {
                animator.SetInteger("Jump", 2);
            }
        } else {
            animator.SetInteger("Jump", 0);
            animator.SetBool("Running", false);
            animator.SetBool("isDead", true);
        }
    }

    void Jump() {
        if ((coyoteTimer > 0 && jumpPressed) || (jumpPressed && isGrounded && canJump)) {
            jumpForce = baseJumpForce;
            coyoteTimer = 0;
            body.velocity = new Vector2(0, baseJumpForce);
            canJump = false;
        }

        if (!isGrounded && body.velocity.y > 0 && !jumpPressed) {
            if (jumpForce > -0.4f) {
                jumpForce -= 0.1f;
            }
            if (jumpForce < -0.4f) {
                jumpForce = -0.4f;
            }
            // body.velocity = new Vector2(body.velocity.x, jumpForce * 0.1f);
            body.AddForce(new Vector2(0, -20f));
        }

        if (isGrounded && !jumpPressed) {
            coyoteTimer = coyoteTiming;
            canJump = true;
        }

        if (!isGrounded) {
            if (coyoteTimer > 0) {
                coyoteTimer -= 0.1f;
            }
        }
    }

    private IEnumerator deathTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDead = false;
        body.transform.position = new Vector2(0, 0);
    }

    public void Die() {
        animator.SetBool("Running", false);
        isDead = true;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        StartCoroutine(deathTimer(baseDeathTimer));
    }
}
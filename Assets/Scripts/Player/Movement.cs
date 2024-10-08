using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float baseSpeed = 2.8f;
    public float runSpeed = 0f;
    public float baseJumpForce = 2.6f;
    public float jumpForce = 2.6f;

    public Sprite jumpSprite;

    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator animator;
    
    public GameObject spriteObj;

    public bool isGrounded;
    public GameObject groundCheckPoint;
    public float groundCheckRadius;
    public LayerMask groundLayer;

    public bool jumpPressed = false;
    public bool canJump = true;
    public float y_velocity = 0;
    public int horizontalPressed = 0;
    private int lastHorizontalPressed = 0;

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
            if (runSpeed < 0) {
                runSpeed = 0;
            }
            body.velocity = new Vector2(lastHorizontalPressed * runSpeed, body.velocity.y);
        }

        if (jumpPressed && isGrounded && canJump) {
            jumpForce = baseJumpForce;
            body.velocity = new Vector2(body.velocity.x, baseJumpForce);
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
            canJump = true;
        }
    }

    void Anim() {
        if (body.velocity.y > 0.1f && !isGrounded) {
            animator.SetInteger("Jump", 1);
        }
        if (body.velocity.y < -0.1f && !isGrounded) {
            animator.SetInteger("Jump", 2);
        }
        if (isGrounded) {
            animator.SetInteger("Jump", 0);
        }
    }
}
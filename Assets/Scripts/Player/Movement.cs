using System.Collections;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {

	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
	private Vector3 m_Velocity = Vector3.zero;

    [Header("Movement Variables")]
    public float baseSpeed = 2.8f;
    public float runSpeed = 0f;
    public float baseJumpForce = 2.6f;
    private float jumpForce = 2.6f;

    private RaycastHit2D leftHit;
    private RaycastHit2D rightHit;

    private float leftPositionX;
    private float rightPositionX;
    public float coyoteTiming = 0;
    private float coyoteTimer = 0;

    public float wallCoyoteTiming = 0;
    private float wallCoyoteTimer = 0;

    public int wallDir;

    public float groundCheckRadius;

    public int horizontalPressed = 0;
    public int verticalPressed = 0;
    public int lastHorizontalPressed = 0;

    public bool isDead = false;
    public float baseDeathTimer = 200;

    public bool isFrozen = false;

    public bool canMove = true;

    public bool edgeCheck;

    public int prot = 0;
    public bool canProt = true;
    public bool protEnabled = false;
    public bool canDash = true;

    [Header("References")]

    public Shockwave shockwave;

    public LayerMask spikeLayer;

    public Transform edge_position;
    public Sprite jumpSprite;

    public BoxCollider2D player_colider;

    private Rigidbody2D body;
    private SpriteRenderer sr;
    private Animator animator;
    
    public GameObject spriteObj;

    private bool isGrounded;
    public GameObject groundCheckPoint;
    
    public LayerMask groundLayer;
    
    public Transform wallCheck;
    public bool isWallTouch;
    public bool isSliding;
    public float wallSlidingSpeed;

    public float wallJumpDuration;
    public Vector2 wallJumpForce;
    public bool wallJumping;
    public bool forcing;

    private bool jumpPressed = false;
    public bool canJump = true;

    public Vector2 checkPoint;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        sr = spriteObj.GetComponent<SpriteRenderer>();
        animator = spriteObj.GetComponent<Animator>();
        rightPositionX = player_colider.bounds.max.x + .1f;
        leftPositionX = player_colider.bounds.min.x - .1f;
    }

    void Update() {

        Anim();

        if (canMove){

            if (Input.GetKeyDown(KeyCode.LeftShift) && canProt){
                canProt = false;
                
                if (prot == 1) {
                    shockwave.CallShockWave();
                    prot = 0;
                } else {
                    shockwave.UnCallShockWave();
                    prot = 1;
                }
            } else if (protEnabled) {
                if(shockwave.dis <= 0.08f || shockwave.dis >= 0.5f){
                    canProt = true;
                }   
            }

            if(isGrounded) canDash = true;

            if (Input.GetKeyDown(KeyCode.Space)) jumpPressed = true;
            if (Input.GetKeyUp(KeyCode.Space)) jumpPressed = false;
            
            if (Input.GetKey(KeyCode.A)) horizontalPressed = -1;
            if (Input.GetKey(KeyCode.D)) horizontalPressed = 1;
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) horizontalPressed = 0;
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) horizontalPressed = 0;

            if (Input.GetKey(KeyCode.S)) verticalPressed = -1;
            if (Input.GetKey(KeyCode.W)) verticalPressed = 1;
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.W)) verticalPressed = 0;
            if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W)) verticalPressed = 0;


        } else {
            jumpPressed = false;
        }

        if (horizontalPressed != 0) {
            animator.SetBool("Running", true);
        }
        else{
            animator.SetBool("Running", false);
        }
    }

    void FixedUpdate() {
        if (!isDead) {
            SpikeCheck();
            Jump();
            Move();
            WallJump();
            if(forcing) {
                body.velocity = new Vector2(body.velocity.x,6f);
            }
        }
    }

    void WallJump() {
        isWallTouch = Physics2D.OverlapBox(wallCheck.position, new Vector2(.19f, 0.44f), 0, groundLayer);

        if(isWallTouch && !isGrounded && horizontalPressed != 0) {
            isSliding = true;
        } else {
            isSliding = false;
        }
        if(isSliding) {
            wallCoyoteTimer = wallCoyoteTiming;
            body.velocity = new Vector2 (body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
            wallDir = horizontalPressed;
        }
        if(wallJumping) {
            body.velocity = new Vector2(-wallDir*wallJumpForce.x, wallJumpForce.y);
        }

        if (!isSliding) {
            if (wallCoyoteTimer > 0) {
                wallCoyoteTimer -= 0.1f;
            }
        }
    }

    void Move() {
        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.transform.position, groundCheckRadius, groundLayer);
        if(!wallJumping || !forcing){
            if (horizontalPressed != 0) {
                if (runSpeed < baseSpeed) {
                    runSpeed += 0.5f;
                }
                lastHorizontalPressed = horizontalPressed;
                // body.velocity = new Vector2(horizontalPressed * runSpeed, body.velocity.y);
                
                Vector3 targetVelocity = new Vector2(horizontalPressed * runSpeed, body.velocity.y);
                body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

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
                Vector3 targetVelocity = new Vector2(horizontalPressed * runSpeed, body.velocity.y);
                body.velocity = Vector3.SmoothDamp(body.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            }
            }
        }

    void SpikeCheck() {
        RaycastHit2D raycastDown = Physics2D.Raycast(transform.position, Vector2.down, 0.25f, spikeLayer);

        if(raycastDown && !forcing) {
            forcing = true;
            body.velocity = new Vector2(0, 0);
            canDash = false;
            
            Invoke("StopForce", 0.1f);
            shockwave.UnCallShockWave();
            prot = 1;
        }
    }

    void Anim() {

        edgeCheck = Physics2D.OverlapBox(edge_position.position, new Vector2(.03f, 0.07f), 0, groundLayer);

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
            if(body.velocity.y < 0 && isSliding) {
                animator.SetBool("Wall Slide", true);
            }
            else {
                animator.SetBool("Wall Slide", false);
            }
            if(edgeCheck || !isGrounded) {
                animator.SetBool("onEdge", false);
            } else if(!edgeCheck && isGrounded) {
                animator.SetBool("onEdge", true);
            }
        } else {
            animator.SetInteger("Jump", 0);
            animator.SetBool("Running", false);
            animator.SetBool("isDead", true);
        }
    }

    void Jump() {
        if ((coyoteTimer > 0 && jumpPressed && !forcing) || (jumpPressed && isGrounded && canJump && !forcing)) {
            jumpForce = baseJumpForce;
            coyoteTimer = 0;
            canJump = false;
            body.velocity = new Vector2(body.velocity.x, baseJumpForce);
        }
        
        if ((wallCoyoteTimer > 0 && jumpPressed && canJump && !forcing) || (jumpPressed && canJump && isSliding && !forcing)) {
            wallCoyoteTimer = 0;
            wallJumping = true;
            canJump = false;
            Invoke("StopWallJump", wallJumpDuration);
        }

        if (!isGrounded && body.velocity.y > 0 && !jumpPressed && !forcing) {
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
        } else if (isSliding && !jumpPressed) {
            canJump = true;
        }

        if (!isGrounded) {
            if (coyoteTimer > 0) {
                coyoteTimer -= 0.1f;
            }
        }
    }

    void StopWallJump() {
        wallJumping = false;
    }

    void StopForce() {
        forcing = false;
    }

    public IEnumerator deathTimer(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        isDead = false;
        body.transform.position = checkPoint;
    }

    public void Die() {
        if(prot == 1) {
            animator.SetBool("Running", false);
            isDead = true;
            body.constraints = RigidbodyConstraints2D.FreezeAll;
            StartCoroutine(deathTimer(baseDeathTimer));
        } else {
            shockwave.UnCallShockWave();
            prot = 1;
        }
        
    }
}
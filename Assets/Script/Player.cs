using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Config
    public float speed;
    public float jumpSpeed;
    public float climbSpeed;
    //State

    //Cached component references
    Rigidbody2D rb;
    Animator anim;
    CapsuleCollider2D body_c2D;
    BoxCollider2D feet_c2D;
    float gravityScale_Start;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body_c2D = GetComponent<CapsuleCollider2D>();
        feet_c2D = GetComponent<BoxCollider2D>();
        gravityScale_Start = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        Jump();
        FlipPlayer();
        Climb();
    }

    private void Run()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 playerVelocity = new Vector2(moveX * speed, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        anim.SetBool("Running", playerHasHorizontalSpeed);   
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") &&
           feet_c2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            rb.velocity += jumpVelocityToAdd;
        }
     /* if (Input.GetButtonDown("Jump"))
        {
            Vector2 addJumpVelocity = new Vector2(0f, jumpSpeed);
            rb.velocity += addJumpVelocity;
        } */
    }

    private void Climb()
    {

        if (!feet_c2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            anim.SetBool("Climbing", false);
            rb.gravityScale = gravityScale_Start;
            return;
        }

        float moveY = Input.GetAxisRaw("Vertical");
        Vector2 climbVelocity = new Vector2(rb.velocity.x, moveY * climbSpeed);
        rb.velocity = climbVelocity;
        rb.gravityScale = 0.001f;

        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        anim.SetBool("Climbing", playerHasVerticalSpeed);


    }

    private void FlipPlayer()
    {
        bool ifPlayerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (ifPlayerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);
        }
    }
}

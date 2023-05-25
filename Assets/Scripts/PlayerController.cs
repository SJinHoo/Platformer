using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [SerializeField] LayerMask groundLayer;

    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float movePower;
    [SerializeField]
    private float jumpPower;
    private bool isGround;
    private bool hasJumped;


    private Vector2 inputDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Move();
        
    }

    private void FixedUpdate()
    {
        GroundCheck(); 
    }

    public void Move() 
    {
        if(inputDir.x < 0 && rb.velocity.x >  -maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        else if (inputDir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower);
    }

    public void Jump()
    {
        if (!hasJumped && isGround)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
        }    
    }
    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
        animator.SetFloat("MoveSpeed", Mathf.Abs(inputDir.x));
        if (inputDir.x > 0)
            spriteRenderer.flipX = false;
        else if (inputDir.x < 0)
            spriteRenderer.flipX = true;
    }

    private void OnJump(InputValue value)
    {
        if (!value.isPressed)
            return;
            Jump();  
    }

    private void GroundCheck()
    {
        RaycastHit2D hitGround = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer); //LayerMask.GetMask()); ;
        
        if(hitGround.collider != null)
        {
            hasJumped = false;
            isGround = true;
            animator.SetBool("IsGround", true);
            Debug.DrawRay(transform.position, new Vector3(hitGround.point.x, hitGround.point.y, 0) - transform.position, Color.red);
        }
        else
        {
            isGround = false;
            animator.SetBool("IsGround", false);
            Debug.DrawRay(transform.position, Vector3.down * 1.5f, Color.green);
        }
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("IsGround", true);
        jumpCount = 0;
        // if (collision.gameObject.layer == LayerMask.GetMask("Monster")) ;
        // 몬스터와 충돌시
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsGround", false);
        
    }
    */
    
}

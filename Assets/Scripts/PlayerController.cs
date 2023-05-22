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
    

    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float movePower;
    [SerializeField]
    private float jumpPower;
    private int jumpCount = 0;



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

    public void Move() 
    {
        if(inputDir.x < 0 && rb.velocity.x >  -maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower, ForceMode2D.Force);
        else if (inputDir.x > 0 && rb.velocity.x < maxSpeed)
            rb.AddForce(Vector2.right * inputDir.x * movePower);
    }

    public void Jump()
    {
        if (jumpCount < 1)
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            animator.SetBool("Jump",true);
            jumpCount++;
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

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetBool("IsGround", true);
        jumpCount = 0;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        animator.SetBool("IsGround", false);
        
    }
}

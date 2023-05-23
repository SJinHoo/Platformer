using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    // ���͸� �����Ҷ� �������ִ� �������� �����̵��� �ϰ��ϱ�
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rb;
    private Animator animator;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
   

    void Update()
    {
        Move();
        if (!IsGroundExits())
           Turn();
        
    }

    public void Move()
    {
        rb.velocity = new Vector2(transform.right.x * -1 * moveSpeed, rb.velocity.y);
    }
    public void Turn()
    {
        transform.Rotate(Vector3.up, 180);
    }
    private bool IsGroundExits()
    {
        Debug.DrawRay(groundCheckPoint.position, Vector2.down, Color.red);
        return Physics2D.Raycast(groundCheckPoint.position, Vector2.down, 1f, groundMask);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]private float moveForce = 10f;//默认移动速度
    [SerializeField]private float jumpForce = 15f;//默认跳跃速度
    [SerializeField]private float fallForce = 5f;
    [SerializeField]private LayerMask Ground;//地面layer
    private bool isGrounded;//用于判断是否在地面上
    private float moveProportionX;//水平方向的速度比例
    private Animator playerAnimator;//对象上状态机
    private Rigidbody2D rigidbody2D;//对象上的刚体
    private SpriteRenderer spriteRenderer;//对象上的精灵渲染器
    private bool isJumping = false;
    void Awake()
    {
     spriteRenderer = GetComponent<SpriteRenderer>();
     playerAnimator = GetComponent<Animator>();
     rigidbody2D = GetComponent<Rigidbody2D>();
     Ground = LayerMask.GetMask("Ground");
    }
    void Update()
    {
        MovePlayer();
        JumpPlayer();
        SpeedLimit();
    }
    //移动
    private void MovePlayer()
    {
        moveProportionX = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(moveForce * moveProportionX,rigidbody2D.velocity.y);//设置速度
        spriteRenderer.flipX = !(moveProportionX >= 0);//判断是否需要反转
        playerAnimator.SetBool("isMove",Mathf.Abs(moveProportionX) > 0);//根据是否移动来更改状态机待机或者奔跑
    }
    //跳跃
    private float coyoteTimeCounter;//土狼时间计数器
    [SerializeField]private float coyoteTime = 0.2f;//土狼时间
    private float riseTimeCounter;//上升时间计数器
    [SerializeField]private float riseTime = 0.15f;//到达最大高度的时间
    private float jumpBufferCounter;//跳跃缓冲计时
    private float jumpBufferTime = 0.15f;//跳跃缓冲
    
    private void JumpPlayer()
    {
        //判断是否再地面上
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, Ground);
        //线性跳跃
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        if (!isJumping)
        {
            rigidbody2D.velocity -= new Vector2(0f, -Physics2D.gravity.y * Time.deltaTime * fallForce);
            playerAnimator.SetBool("isFall", true);
        }
        //跳跃缓冲
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        //输入jump执行跳跃//土狼时间
        coyoteTimeCounter += Time.deltaTime;
        if ( Input.GetButton("Jump") && (isGrounded || coyoteTimeCounter <= coyoteTime) && jumpBufferCounter > 0f)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce);
            playerAnimator.SetTrigger("Jump");
            jumpBufferCounter = 0f;
        }
        
        //改变重力的时刻
        riseTimeCounter += Time.deltaTime;
        if (riseTimeCounter >= riseTime)
        {
            rigidbody2D.gravityScale = 5;
            playerAnimator.SetBool("isFall", true);
        }
            
        //在地面时的处理
        if (isGrounded)
        {
            coyoteTimeCounter = 0;
            riseTimeCounter = 0;
            playerAnimator.SetBool("isFall", false);
            rigidbody2D.gravityScale = 1;
        }
    }
    //速度限制
    [SerializeField]private float maxSpeed = 20f;
    private void SpeedLimit()
    {
        if (rigidbody2D.velocity.y <= -maxSpeed)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -maxSpeed);
        }
    }
}
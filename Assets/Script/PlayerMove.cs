using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    //初始化默认移动速度
    [SerializeField]private float moveSpeed = 10f;
    
    //初始化默认跳跃速度
    [SerializeField]private float jumpSpeed = 15f;
    
    //初始化默认下落速度
    [SerializeField]private float fallSpeed = 5f;
    
    //地面layer
    [SerializeField]private LayerMask Ground;
    
    //用于判断是否在地面上
    private bool isGrounded;
    
    //水平方向的速度比例
    private float moveProportionX;
    
    //对象上的刚体
    private new Rigidbody2D rigidbody2D;
    
    //对象上的精灵渲染器
    private SpriteRenderer spriteRenderer;
    
    //用于判断是否跳起
    private bool isJumping = false;
    
    void Awake()
    {
     spriteRenderer = GetComponent<SpriteRenderer>();
     rigidbody2D = GetComponent<Rigidbody2D>();
     Ground = LayerMask.GetMask("Ground");
    }
    
    void Update()
    {
        MovePlayer();
        PlayerJump();
        SpeedLimit();
    }
    
    /// <summary>
    /// 移动
    /// </summary>
    private void MovePlayer()
    {
        moveProportionX = Input.GetAxis("Horizontal");
        //设置速度
        rigidbody2D.velocity = new Vector2(moveSpeed * moveProportionX,rigidbody2D.velocity.y);
        //判断是否需要反转
        spriteRenderer.flipX = !(moveProportionX >= 0);
    }
    
    //土狼时间计数器
    private float coyoteTimeCounter;
    
    //土狼时间
    [SerializeField]private float coyoteTime = 0.2f;
    
    //上升时间计数器
    private float riseTimeCounter;
    
    //到达最大高度的时间
    [SerializeField]private float riseTime = 0.15f;
    
    //跳跃缓冲计时
    private float jumpBufferCounter;
    
    //跳跃缓冲
    [SerializeField]private float jumpBufferTime = 0.15f;
    
    /// <summary>
    /// 跳跃
    /// </summary>
    private void PlayerJump()
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
            //增加向下的力
            rigidbody2D.velocity -= new Vector2(0f, -Physics2D.gravity.y * Time.deltaTime * fallSpeed);
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
            //执行跳越
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpSpeed);
            //清空跳越缓冲
            jumpBufferCounter = 0f;
        }
        
        //改变重力的时刻
        riseTimeCounter += Time.deltaTime;
        if (riseTimeCounter >= riseTime)
        {
            rigidbody2D.gravityScale = fallSpeed;
        }
            
        //在地面时的处理
        if (isGrounded)
        {
            //清空土狼时间计数器
            coyoteTimeCounter = 0;
            //清空上升时间计数器
            riseTimeCounter = 0;
            //清空重力倍数
            rigidbody2D.gravityScale = 1;
        }
    }
    
    //最大速度
    [SerializeField]private float maxSpeed = 20f;
    
    /// <summary>
    /// 速度限制
    /// </summary>
    private void SpeedLimit()
    {
        if (rigidbody2D.velocity.y <= -maxSpeed)
        {
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -maxSpeed);
        }
    }
}
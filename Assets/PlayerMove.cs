using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMove : MonoBehaviour
{
    public float deMoveForce = 10f;//默认移动速度
    public float deJumpForce = 15f;//默认跳跃速度
    public LayerMask Ground;//地面layer
    private bool isGrounded;//用于判断是否在地面上
    //水平方向的速度比例
    private float moveProportionX;
    public Animator m_playerAnimator;//对象上状态机
    public Rigidbody2D m_rigidbody2D;//对象上的刚体
    public SpriteRenderer spriteRenderer;//对象上的精灵渲染器
    void Start()
    {
     spriteRenderer = GetComponent<SpriteRenderer>();
     m_playerAnimator = GetComponent<Animator>();
     m_rigidbody2D = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        MovePlayer();
        JumpPlayer();
    }
    //移动
    private void MovePlayer()
    {
        moveProportionX = Input.GetAxis("Horizontal");
        m_rigidbody2D.velocity = new Vector2(deMoveForce * moveProportionX,m_rigidbody2D.velocity.y);//设置速度
        spriteRenderer.flipX = !(moveProportionX >= 0);//判断是否需要反转
        m_playerAnimator.SetBool("isMove",Mathf.Abs(moveProportionX) > 0);//根据是否移动来更改状态机待机或者奔跑
    }
    //跳跃
    private float time;//用于计数器
    public float downTime = 0.15f;//改变重力的时间
    private void JumpPlayer()
    {
        //判断是否再地面上
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.2f, Ground);
        //输入jump执行跳跃
        if (Input.GetButtonDown("Jump") && (isGrounded || time <= 0.05f))
        {
            m_rigidbody2D.velocity = new Vector2(m_rigidbody2D.velocity.x, deJumpForce);
            m_playerAnimator.SetTrigger("Jump");
        }
        
        //改变重力的时刻
        if (time >= downTime)
            m_rigidbody2D.gravityScale = 5;
        //降落后接触地面时的处理 在地面时的处理
        if (isGrounded)
        {
            m_rigidbody2D.gravityScale = 1;
            time = 0;
        }
        //计时器
        time += Time.deltaTime;
    }
}

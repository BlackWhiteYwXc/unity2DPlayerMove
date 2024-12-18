using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsFall = Animator.StringToHash("isFall");
    private Animator playerAnimator;//初始化状态机
    private Rigidbody2D playerRigidbody2D;
    private LayerMask Ground;
    private bool isGround;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        Ground = LayerMask.GetMask("Ground");
    }
    
    void Update()
    {
        isGround = Physics2D.OverlapCircle(transform.position, 0.2f, Ground);
        isMove();
        isFall();
        isJump();
    }
    //判断是否在移动
    private void isMove()
    {
        if(playerRigidbody2D.velocity.x != 0)
        {
            playerAnimator.SetBool("isMove", false);
        }
        else
        {
            playerAnimator.SetBool("isMove", true);
        }
    }
    //判断是否在下落
    private void isFall()
    {
            if (playerRigidbody2D.gravityScale != 1)
            {
                playerAnimator.SetBool("isFall",true);
            }
            else if(isGround)
            {
                playerAnimator.SetBool("isFall",false);
            }
    }
    //判断是否跳越
    private void isJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerAnimator.SetTrigger("Jump");
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform PlayerTransform;
    //移动速度
    private Vector3 velocity = Vector3.zero;
    //摄像机偏移距离
    public Vector3 positionOffset = new Vector3(-5,0,0);
    [Range(0,1)]
    public float smoothTime = 0.1f;
    [Header("摄像机限制范围(最小值，最大值)")]
    public Vector2 xLimit;//X方向限制范围
    public Vector2 yLimit;//Y方向限制范围

    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private Vector3 targetPosition;
    private void LateUpdate()
    {
        targetPosition = PlayerTransform.position + positionOffset;
        targetPosition = new Vector3(Mathf.Clamp(targetPosition.x,xLimit.x,xLimit.y),Mathf.Clamp(targetPosition.y,yLimit.x,yLimit.y),-10);
        this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, smoothTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlatform : MonoBehaviour
{
    // 一个变量用来存储当前玩家所在的平台
    private Transform currentPlatform;

    // 一个变量用来存储玩家相对于平台的位置偏移
    private Vector3 offset;

    // 一个变量用来存储玩家是否正在尝试移动
    private bool isMoving;

    // 当玩家进入碰撞时调用
    private void OnCollisionEnter(Collision collision)
    {
        // 如果碰撞的物体有"platform"标签
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // 将当前平台设为碰撞的物体的变换组件
            currentPlatform = collision.transform;

            // 计算玩家相对于平台的位置偏移
            offset = transform.position - currentPlatform.position;

            // 将玩家作为平台的子物体
            transform.parent = currentPlatform;
        }
    }

    // 当玩家离开碰撞时调用
    private void OnCollisionExit(Collision collision)
    {
        // 如果碰撞的物体有"platform"标签
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // 将当前平台设为空
            currentPlatform = null;

            // 将玩家从平台的子物体中移除
            transform.parent = null;
        }
    }

    // 每帧更新时调用
    private void Update()
    {
        // 如果当前平台不为空
        if (currentPlatform != null)
        {
            // 更新玩家的位置为平台的位置加上偏移量
            transform.position = currentPlatform.position + offset;
        }

        // 检测玩家是否按下了水平方向键
        isMoving = Input.GetAxis("Horizontal") != 0;

        // 如果玩家正在尝试移动，并且当前平台不为空
        if (isMoving && currentPlatform != null)
        {
            // 将当前平台设为空
            currentPlatform = null;

            // 将玩家从平台的子物体中移除
            transform.parent = null;
        }
    }
}
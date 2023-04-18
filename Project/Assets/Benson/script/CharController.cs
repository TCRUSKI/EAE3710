using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CharController : MonoBehaviour
{
    private CharacterController cc;//角色控制器
    public float MoveSpeed;//移动速度
    [SerializeField] private float gravity = 1f;//重力(游戏制作时的标准化代码)
    public float JumpHeight = 4f; //跳跃高度

    //private Rigidbody rb;//废弃原方案

    private float H_move, V_move;//垂直和水平移动
    private Vector3 Direction;
    private Vector3 Velocity;

    public Transform groundCheak;//地面检测（位置）
    public float cheakRadius;//检测半径
    public LayerMask groundLayer;//层级检测
    public bool isGround;//检测是否落地

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGround = Physics.CheckSphere(groundCheak.position, cheakRadius, groundLayer);//落地检测

        if (isGround && Velocity.y < 0)
        {
            Velocity.y = -2f;
            Debug.Log("接触地面了");
        }

        H_move = Input.GetAxis("Horizontal") * MoveSpeed;
        V_move = Input.GetAxis("Vertical") * MoveSpeed;

        Direction = transform.forward * V_move + transform.right * H_move;
        cc.Move(Direction * Time.deltaTime);//平面移动

        Velocity.y -= gravity * Time.deltaTime;
        cc.Move(Velocity * Time.deltaTime);//重力

        if (Input.GetButton("Jump") && isGround)
        {
            Velocity.y = JumpHeight;
        }
        //按键检测(空格)跳跃

        /*
         if (Input.GetKey(KeyCode.W)) //顾名思义，获取按键W
         {
             rb.velocity = transform.forward * MoveSpeed; //这个方案适用于使用rigidbody时，rigidbody通常使用于2d项目
        }
         rb.velocity += Vector3.down * gravity; */
    }
}
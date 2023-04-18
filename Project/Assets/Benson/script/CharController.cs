using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class CharController : MonoBehaviour
{
    private CharacterController cc;//��ɫ������
    public float MoveSpeed;//�ƶ��ٶ�
    [SerializeField] private float gravity = 1f;//����(��Ϸ����ʱ�ı�׼������)
    public float JumpHeight = 4f; //��Ծ�߶�

    //private Rigidbody rb;//����ԭ����

    private float H_move, V_move;//��ֱ��ˮƽ�ƶ�
    private Vector3 Direction;
    private Vector3 Velocity;

    public Transform groundCheak;//�����⣨λ�ã�
    public float cheakRadius;//���뾶
    public LayerMask groundLayer;//�㼶���
    public bool isGround;//����Ƿ����

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGround = Physics.CheckSphere(groundCheak.position, cheakRadius, groundLayer);//��ؼ��

        if (isGround && Velocity.y < 0)
        {
            Velocity.y = -2f;
            Debug.Log("�Ӵ�������");
        }

        H_move = Input.GetAxis("Horizontal") * MoveSpeed;
        V_move = Input.GetAxis("Vertical") * MoveSpeed;

        Direction = transform.forward * V_move + transform.right * H_move;
        cc.Move(Direction * Time.deltaTime);//ƽ���ƶ�

        Velocity.y -= gravity * Time.deltaTime;
        cc.Move(Velocity * Time.deltaTime);//����

        if (Input.GetButton("Jump") && isGround)
        {
            Velocity.y = JumpHeight;
        }
        //�������(�ո�)��Ծ

        /*
         if (Input.GetKey(KeyCode.W)) //����˼�壬��ȡ����W
         {
             rb.velocity = transform.forward * MoveSpeed; //�������������ʹ��rigidbodyʱ��rigidbodyͨ��ʹ����2d��Ŀ
        }
         rb.velocity += Vector3.down * gravity; */
    }
}
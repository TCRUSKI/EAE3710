using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlatform : MonoBehaviour
{
    // һ�����������洢��ǰ������ڵ�ƽ̨
    private Transform currentPlatform;

    // һ�����������洢��������ƽ̨��λ��ƫ��
    private Vector3 offset;

    // һ�����������洢����Ƿ����ڳ����ƶ�
    private bool isMoving;

    // ����ҽ�����ײʱ����
    private void OnCollisionEnter(Collision collision)
    {
        // �����ײ��������"platform"��ǩ
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // ����ǰƽ̨��Ϊ��ײ������ı任���
            currentPlatform = collision.transform;

            // ������������ƽ̨��λ��ƫ��
            offset = transform.position - currentPlatform.position;

            // �������Ϊƽ̨��������
            transform.parent = currentPlatform;
        }
    }

    // ������뿪��ײʱ����
    private void OnCollisionExit(Collision collision)
    {
        // �����ײ��������"platform"��ǩ
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            // ����ǰƽ̨��Ϊ��
            currentPlatform = null;

            // ����Ҵ�ƽ̨�����������Ƴ�
            transform.parent = null;
        }
    }

    // ÿ֡����ʱ����
    private void Update()
    {
        // �����ǰƽ̨��Ϊ��
        if (currentPlatform != null)
        {
            // ������ҵ�λ��Ϊƽ̨��λ�ü���ƫ����
            transform.position = currentPlatform.position + offset;
        }

        // �������Ƿ�����ˮƽ�����
        isMoving = Input.GetAxis("Horizontal") != 0;

        // ���������ڳ����ƶ������ҵ�ǰƽ̨��Ϊ��
        if (isMoving && currentPlatform != null)
        {
            // ����ǰƽ̨��Ϊ��
            currentPlatform = null;

            // ����Ҵ�ƽ̨�����������Ƴ�
            transform.parent = null;
        }
    }
}
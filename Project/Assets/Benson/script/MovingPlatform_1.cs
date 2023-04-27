/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Vector3> coordinates;
    public List<Vector3> distance;
    public int step;

    void Start(){
        step = 0;
    }

    void FixedUpdate()
    {
        if(transform.position == coordinates[step]){
            if(step == coordinates.Count - 1){
                step = 0;
            } else {
                step++;
            }
        }
        transform.position = transform.position + distance[step];
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform_1: MonoBehaviour
{
    public Transform start; // ���
    public Transform end; // �յ�
    public float speed; // �ٶ�
    private Transform target; // Ŀ��

    void Start()
    {
        target = end; // ��ʼĿ��Ϊ�յ�
    }

    void Update()
    {
        float step = speed * Time.deltaTime; // ÿ֡�ƶ��ľ���
        transform.position = Vector3.MoveTowards(transform.position, target.position, step); // ��Ŀ���ƶ�
        if (transform.position == target.position) // �������Ŀ��
        {
            if (target == end) // ���Ŀ�����յ�
            {
                target = start; // �л�Ŀ��Ϊ���
            }
            else // ���Ŀ�������
            {
                target = end; // �л�Ŀ��Ϊ�յ�
            }
        }
    }
}
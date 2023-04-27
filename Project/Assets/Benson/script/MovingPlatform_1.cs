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
    public Transform start; // 起点
    public Transform end; // 终点
    public float speed; // 速度
    private Transform target; // 目标

    void Start()
    {
        target = end; // 初始目标为终点
    }

    void Update()
    {
        float step = speed * Time.deltaTime; // 每帧移动的距离
        transform.position = Vector3.MoveTowards(transform.position, target.position, step); // 向目标移动
        if (transform.position == target.position) // 如果到达目标
        {
            if (target == end) // 如果目标是终点
            {
                target = start; // 切换目标为起点
            }
            else // 如果目标是起点
            {
                target = end; // 切换目标为终点
            }
        }
    }
}
using System.Collections;
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

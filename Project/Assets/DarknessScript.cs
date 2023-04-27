using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DarknessScript : MonoBehaviour
{
    Dictionary <int, (int,int)> DARKNESS;





    MeshRenderer mr;
    Color col;
    // Start is called before the first frame update
    void Start()
    {
        DARKNESS = new Dictionary<int, (int,int)>();
        DARKNESS[2] = (50,50);
        mr = GetComponent<MeshRenderer>();
        col = mr.material.color;
    }


    void Update()
    {
        float y = transform.position.y;
        float z = transform.position.z;
        int activeScene = SceneManager.GetActiveScene().buildIndex;
        (int,int) tuple = DARKNESS[activeScene];
        int item1 = tuple.Item1;
        int item2 = tuple.Item2;

        float value1 = y/item1;
        float value2 = z/item2;
        col.a = Mathf.Max(value1, value2);
        mr.material.color = col;
    }
}

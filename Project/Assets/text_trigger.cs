using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class text_trigger : MonoBehaviour
{
    public GameObject UIObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().gameObject.tag == "CharTag")
        {
            UIObject.SetActive(true);
            Debug.Log("trigged");
        }
    }

    private void OnTriggerExit(Collider other)
    {
            UIObject.SetActive(false);
            Debug.Log("trigged");
    }


}

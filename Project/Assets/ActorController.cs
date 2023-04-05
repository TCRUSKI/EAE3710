using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorController : MonoBehaviour
{
    public BoxCollider bc2d;
    public Rigidbody rb;
    private RigidbodyConstraints defaultConstraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    public bool ledge {get; protected set;}
    public bool ladder {get; protected set;}

    public float framesSinceGrounced {get; protected set;}

    public float framesSinceClimb {get; protected set;}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc2d = GetComponent<BoxCollider>();
        framesSinceClimb = -1;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        bool shiftDown = Input.GetKey(KeyCode.LeftShift);
    
        if(Math.Sign(Input.GetAxis("Horizontal")) != Math.Sign(rb.velocity.z) && Math.Sign(rb.velocity.z) != 0 && Input.GetAxis("Horizontal") != 0){
            //Debug.Log("Direction: " + Math.Sign(Input.GetAxis("Horizontal")) + "\nVelocity: " + Math.Sign(rb.velocity.z));

            rb.AddForce(new Vector3(0,0,Math.Sign(Input.GetAxis("Horizontal"))*50), ForceMode.Acceleration);



        } else {
            if(Math.Sign(Input.GetAxis("Horizontal")) > 0) {
                if(!shiftDown){
                    if(Math.Abs(rb.velocity.z) < 1.4){
                        rb.AddForce(new Vector3(0,0,5), ForceMode.Acceleration);
                    } else {
                        rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                    }
                } else if(Math.Abs(rb.velocity.z) < 5.4) {
                    rb.AddForce(new Vector3(0,0,20), ForceMode.Acceleration);
                } else {
                        rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                }
            } else if (Math.Sign(Input.GetAxis("Horizontal")) < 0) {
                if(!shiftDown){
                    if(Math.Abs(rb.velocity.z) < 1.4){
                        rb.AddForce(new Vector3(0,0,-5), ForceMode.Acceleration);
                    } else {
                        rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                    }
                } else if(Math.Abs(rb.velocity.z) < 5.4) {
                    rb.AddForce(new Vector3(0,0,-20), ForceMode.Acceleration);
                } else {
                        rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                }
            }
        }
        
        
        
        

        if (Input.GetKey(KeyCode.Space)){
            if(IsGrounded())
            {
                rb.velocity = rb.velocity + new Vector3(0, 2f, 0);
            } else if(framesSinceGrounced < 5){
                rb.velocity = rb.velocity + new Vector3(0, .5f, 0);
            } else if(ledge){
                ledge = false;
                framesSinceGrounced = 0;
                rb.constraints = defaultConstraints;
                rb.velocity = rb.velocity + new Vector3(0, 6f, 0);
            }
        }

        if(IsGrounded()){
            framesSinceGrounced = 0;
        } else {
            framesSinceGrounced++;
        }
        if(framesSinceClimb != -1){
            framesSinceClimb++;
        } 
        if(framesSinceClimb > 60) {
            framesSinceClimb = -1;
        }

        if(framesSinceClimb == -1){
            RaycastHit hit;
            bool flag = Physics.Raycast(bc2d.bounds.center + new Vector3(0, bc2d.bounds.extents.y/2, 0), Vector3.forward, out hit, bc2d.bounds.extents.z + 0.05f);
            if(flag && hit.collider.gameObject.tag == "LedgeCollisionBox"){
                rb.velocity = new Vector3(0,0,0);
                ledge = true;
                rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
                framesSinceClimb = 0;
            }
        }

        if(ladder){
            if(Math.Sign(Input.GetAxis("Vertical")) > 0) {
                transform.position = transform.position + new Vector3(0, .1f, 0);
            } else if (Math.Sign(Input.GetAxis("Vertical")) < 0){
                transform.position = transform.position + new Vector3(0, -.1f, 0);
            }
        }

        
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<Collider>().gameObject.tag == "LoadSceneCollisionBox"){
            SceneManager.LoadScene(collision.GetComponent<Collider>().gameObject.GetComponent<LoadSceneCollisionBox>().Scene);
        }
        if(collision.GetComponent<Collider>().gameObject.tag == "Ladder"){
            ladder = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        }
    }

    void OnTriggerExit(Collider collision){
        if(collision.GetComponent<Collider>().gameObject.tag == "Ladder"){
            ladder = false;
            rb.constraints = defaultConstraints;
        }
    }
    

    private bool IsGrounded() 
    { 
        RaycastHit hit;
        bool flagL = Physics.Raycast(bc2d.bounds.center - new Vector3(0, bc2d.bounds.extents.z, 0), Vector3.down, out hit, bc2d.bounds.extents.y + 0.05f);
        bool flagR = Physics.Raycast(bc2d.bounds.center + new Vector3(0, bc2d.bounds.extents.z, 0), Vector3.down, out hit, bc2d.bounds.extents.y + 0.05f);
        return flagL || flagR;
    }


}

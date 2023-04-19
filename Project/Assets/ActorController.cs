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

    protected GameObject characterArt;
    protected AudioSource audioSource;

    public List<Material> climbMats;
    protected int climbMatIndex;
    protected bool climbing;
    protected int climbDir;

    public List<Material> runMats;
    protected int runMatIndex;
    protected bool running;

    public List<Material> jumpMats;
    protected int jumpMatIndex;
    protected bool jumping;
    protected bool falling;

    public Material hangingMat;
    protected bool hanging;
    

    protected Queue<(Texture, int)> animationQueue;
    public Material redMat;
    public Material DefaultMovement;
    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bc2d = GetComponent<BoxCollider>();
        framesSinceClimb = -1;
        characterArt = gameObject.transform.GetChild(1).gameObject;
        audioSource = gameObject.transform.GetChild(3).gameObject.GetComponent<AudioSource>();
        climbMatIndex = 0;
        climbing = false;
        climbDir = 0;
        runMatIndex = 0;
        running = false;
        jumpMatIndex = 0;
        jumping = false;
        falling = false;
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("Jumping: " + jumping);
        Debug.Log("Hanging: " + hanging);
        Debug.Log("Running: " + running);
        Debug.Log("Falling: " + falling);
        Debug.Log("Climbing: " + climbing);
        bool shiftDown = Input.GetKey(KeyCode.LeftShift);


        if(running){
            characterArt.GetComponent<Renderer>().sharedMaterial = runMats[runMatIndex/2];
            if(shiftDown){
                runMatIndex += 2;
            } else {
                runMatIndex++;
            }
            if(runMatIndex >= runMats.Count * 2){
                runMatIndex = 0;
            }
        }

        if(climbing){
            if(!ladder){
                climbing = false;
            } else {
                characterArt.GetComponent<Renderer>().sharedMaterial = climbMats[climbMatIndex/4];
                if(characterArt.transform.localScale.x != climbDir*Math.Abs(characterArt.transform.localScale.x)){
                    characterArt.transform.localScale = new Vector3(climbDir*Math.Abs(characterArt.transform.localScale.x), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                    characterArt.transform.localPosition = new Vector3(characterArt.transform.localPosition.x, characterArt.transform.localPosition.y, -(climbDir*Math.Abs(characterArt.transform.localPosition.z)));
                }
            }
        }

        if(jumping){
            characterArt.GetComponent<Renderer>().sharedMaterial = jumpMats[jumpMatIndex/4];
            jumpMatIndex++;
            if(jumpMatIndex >= jumpMats.Count * 4){
                jumping = false;
                falling = true;
                jumpMatIndex = 0;
            }
        }





    
        if(Math.Sign(Input.GetAxis("Horizontal")) != Math.Sign(rb.velocity.z) && Math.Sign(rb.velocity.z) != 0 && Input.GetAxis("Horizontal") != 0){




            rb.AddForce(new Vector3(0,0,Math.Sign(Input.GetAxis("Horizontal"))*50), ForceMode.Acceleration);
        } else {
            if(Math.Sign(Input.GetAxis("Horizontal")) > 0) {
                if(characterArt.transform.localScale.x != Math.Abs(characterArt.transform.localScale.x) && !climbing){
                    characterArt.transform.localScale = new Vector3(Math.Abs(characterArt.transform.localScale.x), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                    characterArt.transform.localPosition = new Vector3(characterArt.transform.localPosition.x, characterArt.transform.localPosition.y, -Math.Abs(characterArt.transform.localPosition.z));
                }

                if(!running && IsGrounded() && !climbing){
                    running = true;
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x * 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }

                if(!shiftDown){
                    if(IsGrounded() && (audioSource.clip != walkSound || !audioSource.isPlaying)){
                        audioSource.clip = walkSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    if(Math.Abs(rb.velocity.z) < 1.4){
                        rb.AddForce(new Vector3(0,0,5), ForceMode.Acceleration);
                    } else {
                        rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                    }
                } else if(Math.Abs(rb.velocity.z) < 5.4) {
                    if(IsGrounded() && (audioSource.clip != runSound || !audioSource.isPlaying)){
                        audioSource.clip = runSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    rb.AddForce(new Vector3(0,0,20), ForceMode.Acceleration);
                } else {
                    if(IsGrounded() && (audioSource.clip != runSound || !audioSource.isPlaying)){
                        audioSource.clip = runSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                }
            } else if (Math.Sign(Input.GetAxis("Horizontal")) < 0) {
                if(characterArt.transform.localScale.x != -Math.Abs(characterArt.transform.localScale.x) && !climbing){
                    characterArt.transform.localScale = new Vector3(-Math.Abs(characterArt.transform.localScale.x), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                    characterArt.transform.localPosition = new Vector3(characterArt.transform.localPosition.x, characterArt.transform.localPosition.y, Math.Abs(characterArt.transform.localPosition.z));
                }

                if(!running && IsGrounded() && !climbing){
                    running = true;
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x * 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }

                if(!shiftDown){
                    if(IsGrounded() && (audioSource.clip != walkSound || !audioSource.isPlaying)){
                        audioSource.clip = walkSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    if(Math.Abs(rb.velocity.z) < 1.4){
                        rb.AddForce(new Vector3(0,0,-5), ForceMode.Acceleration);
                    } else {
                        rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                    }
                } else if(Math.Abs(rb.velocity.z) < 5.4) {
                    if(IsGrounded() && (audioSource.clip != runSound || !audioSource.isPlaying)){
                        audioSource.clip = runSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    rb.AddForce(new Vector3(0,0,-20), ForceMode.Acceleration);
                } else {
                    if(IsGrounded() && (audioSource.clip != runSound || !audioSource.isPlaying)){
                        audioSource.clip = runSound;
                        audioSource.loop = true;
                        audioSource.Play();
                    }
                    rb.velocity = rb.velocity - new Vector3(0, 0, rb.velocity.z/50);
                }
            } else {
                if(audioSource.isPlaying && audioSource.clip != jumpSound){
                    audioSource.Stop();
                }

                if(running){
                    running = false;
                    runMatIndex = 0;
                    characterArt.GetComponent<Renderer>().sharedMaterial = DefaultMovement;
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x / 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }

                rb.AddForce(new Vector3(0,0,Math.Sign(Input.GetAxis("Horizontal"))*50), ForceMode.Acceleration);
            }
        } 
        
        
        
        

        if (Input.GetKey(KeyCode.Space)){
            if(IsGrounded())
            {
                if(!jumping){
                    jumping = true;
                    if(running){
                        running = false;
                        runMatIndex = 0;
                    }else {
                        characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x * 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                    }
                }
                audioSource.clip = jumpSound;
                audioSource.loop = false;
                audioSource.Play();
                rb.velocity = rb.velocity + new Vector3(0, 4f, 0);
            } else if(framesSinceGrounced < 10){
                rb.velocity = rb.velocity + new Vector3(0, .5f, 0);
            } else if(ledge){
                jumping = true;
                hanging = false;
                ledge = false;
                framesSinceGrounced = 0;
                rb.constraints = defaultConstraints;
                rb.velocity = rb.velocity + new Vector3(0, 6f, 0);
            }
        }

        if(IsGrounded()){
            if(jumping){
                jumping = false;
                jumpMatIndex = 0;
                characterArt.GetComponent<Renderer>().sharedMaterial = DefaultMovement;
                characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x / 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
            }
            if(falling){
                falling = false;
                characterArt.GetComponent<Renderer>().sharedMaterial = DefaultMovement;
                characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x / 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
            }
            if(framesSinceGrounced != 0){
                audioSource.clip = jumpSound;
                audioSource.loop = false;
                audioSource.Play();
            }
            framesSinceGrounced = 0;
        } else {
            if(!falling && !jumping && !climbing && !hanging){
                falling = true;
                characterArt.GetComponent<Renderer>().sharedMaterial = jumpMats[2];
                if(running){
                    running = false;
                    runMatIndex = 0;
                } else {
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x * 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }
            }
            if(audioSource.isPlaying && audioSource.clip != jumpSound){
                audioSource.Stop();
            }
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
                if(!hanging){
                    hanging = true;
                    characterArt.GetComponent<Renderer>().sharedMaterial = hangingMat;
                    if(running){
                        running = false;
                        runMatIndex = 0;
                    }
                    if(jumping){
                        jumping = false;
                        jumpMatIndex = 0;
                    }
                    if(falling){
                        falling = false;
                    }
                }
            }
        }

        if(ladder){
            if(!climbing){
                climbing = true;
                if(running){
                    running = false;
                    runMatIndex = 0;
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x / 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }
                if(jumping){
                    jumping = false;
                    jumpMatIndex = 0;
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x / 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }
                if(falling){
                    falling = false;
                    characterArt.transform.localScale = new Vector3((characterArt.transform.localScale.x / 2), characterArt.transform.localScale.y, characterArt.transform.localScale.z);
                }
            }
            if(Math.Sign(Input.GetAxis("Vertical")) > 0) {
                transform.position = transform.position + new Vector3(0, .1f, 0);
                climbMatIndex++;
                if(climbMatIndex >= climbMats.Count*4){
                    climbMatIndex = 0;
                }
            } else if (Math.Sign(Input.GetAxis("Vertical")) < 0){
                if(!IsGrounded()){
                    transform.position = transform.position + new Vector3(0, -.1f, 0);
                    climbMatIndex--;
                    if(climbMatIndex <= 0){
                        climbMatIndex = climbMats.Count*4 - 1;
                    }
                }
            }
        }

        

        
    }
    void AddAnimation(Texture animation, int length){
        animationQueue.Enqueue((animation, length));
    }





    void OnTriggerEnter(Collider collision)
    {
        if(collision.GetComponent<Collider>().gameObject.tag == "LoadSceneCollisionBox"){
            SceneManager.LoadScene(collision.GetComponent<Collider>().gameObject.GetComponent<LoadSceneCollisionBox>().Scene);
        }
        if(collision.GetComponent<Collider>().gameObject.tag == "Ladder"){
            ladder = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
            climbDir = collision.GetComponent<Collider>().gameObject.GetComponent<LadderScript>().climbDir;
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
        bool flagL = Physics.Raycast(bc2d.bounds.center - new Vector3(0, bc2d.bounds.extents.z, 0), Vector3.down, out hit, bc2d.bounds.extents.y - 0.5f);
        bool flagC = Physics.Raycast(bc2d.bounds.center, Vector3.down, out hit, bc2d.bounds.extents.y - 0.5f);
        bool flagR = Physics.Raycast(bc2d.bounds.center + new Vector3(0, bc2d.bounds.extents.z, 0), Vector3.down, out hit, bc2d.bounds.extents.y - 0.5f);
        return flagL || flagR || flagC;
    }


}

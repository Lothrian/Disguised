using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;

    public float jumpForce = 2.0f;
    private Vector3 jump;
    private Rigidbody rb;
    public bool isGrounded;
    private Animator animator;
    string dir = "right";
    bool hasJumped = false;
    float speedActivationTime = float.MinValue;
    float invisActivationTime = float.MinValue;

    float timer_period = 5;
    public GameObject player;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jump = new Vector3(0.0f, jumpForce, 0.0f);
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        // Reset speed stats after "timer_period" seconds
        if (Time.time > speedActivationTime + timer_period)
        {
            walkSpeed = 1;
            runSpeed = 1;
        }

        // Reset opacity after "timer_period" seconds
        if (Time.time > invisActivationTime + timer_period)
        {
            Color tempColor = player.GetComponent<SkinnedMeshRenderer>().materials[1].color;
            Color tempColor2 = player.GetComponent<SkinnedMeshRenderer>().materials[0].color;
            print(player.GetComponent<SkinnedMeshRenderer>().materials.Length);
            tempColor.a = 1;
            tempColor2.a = 1;
            player.GetComponent<SkinnedMeshRenderer>().materials[1].color = tempColor;
            player.GetComponent<SkinnedMeshRenderer>().materials[0].color = tempColor2;
        }

        // Check that the player is off the ground, have it set to 3 because player.y starts at 3
        // (click on player object and check his initial position)
        if (transform.position.y > 3)
        {
            hasJumped = true;
        }

        // Stuff for jumping and setting jumping animations
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                isGrounded = false;
                rb.AddForce(new Vector3(0.0f, 6.0f, 0.0f), ForceMode.Impulse);
            }
        }

        if (hasJumped) {
            animator.SetBool("is_jumping", true);
        } else {
            animator.SetBool("is_jumping", false);
        }

        // Stuff for moving left/right, (instant rotation)
        if (Input.GetKeyDown("a"))
        {
            if (dir.Equals("right"))
            {
                transform.RotateAround(transform.position, transform.up, 180f);
            }
            dir = "left";
        }
        if (Input.GetKeyDown("d"))
        {
            if (dir.Equals("left"))
            {
                transform.RotateAround(transform.position, transform.up, 180f);
            }
            dir = "right";
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground") && hasJumped){
            hasJumped = false;
        }
        isGrounded = true;


        // Collision detection with boosters, tags need to be set appropriately on objects
        if (collision.gameObject.tag == ("SpeedBooster"))
        {
            collision.gameObject.SetActive(false);
            runSpeed = 5;
            walkSpeed = 5;
            speedActivationTime = Time.time;
        }

        if (collision.gameObject.tag == ("InvisiblePickup"))
        {
            collision.gameObject.SetActive(false);
            Color tempColor = player.GetComponent<SkinnedMeshRenderer>().materials[1].color;
            Color tempColor2 = player.GetComponent<SkinnedMeshRenderer>().materials[0].color;
            print(player.GetComponent<SkinnedMeshRenderer>().materials.Length);
            tempColor.a = 0;
            tempColor2.a = 0;
            player.GetComponent<SkinnedMeshRenderer>().materials[1].color = tempColor;
            player.GetComponent<SkinnedMeshRenderer>().materials[0].color = tempColor2;
            invisActivationTime = Time.time;
        }
    }

    private void FixedUpdate()
    {
        // Stuff for moving left/right + animations
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(move * runSpeed, rb.velocity.y, 0);
        if (rb.velocity.x != 0) {
            animator.SetBool("is_running", true);
        }
        else
        {
            animator.SetBool("is_running", false);
        }
    }


}

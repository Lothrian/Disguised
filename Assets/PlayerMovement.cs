using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed;
    public float jumpForce = 2.0f;
    public float runSpeed;
    private Vector3 jump;
    private Rigidbody rb;
    public bool isGrounded;
    private Animator animator;
    string dir = "right";

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jump = new Vector3(0.0f, jumpForce, 0.0f);
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
      //  animator.setTrigger("standing");
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isGrounded = false;
            rb.AddForce(new Vector3(0.0f, 6.0f, 0.0f), ForceMode.Impulse);
        }
        if (transform.position.y > 3)
        {
            animator.SetBool("is_jumping", true);
        } else
        {
            animator.SetBool("is_jumping", false);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown("a")) {
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

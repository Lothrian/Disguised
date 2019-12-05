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
    float dir = 1;

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
            animator.SetBool("standing", false);
            isGrounded = false;
            rb.AddForce(new Vector3(0.0f, 6.0f, 0.0f), ForceMode.Impulse);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector3(move * runSpeed, rb.velocity.y, 0);
        if (move < 0 && dir > 0 || move > 0 ) {
            transform.RotateAround(transform.position, transform.up, 180f);
        }
        if (rb.velocity.x != 0)
        {
            animator.SetBool("standing", false);
        }
        else
        {
            animator.SetBool("standing", true);
        }
    }


}

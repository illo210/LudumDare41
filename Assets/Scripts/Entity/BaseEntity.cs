using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{

    float speed = 1f;
    Rigidbody rb;
    bool isGrounded = false;
    bool hasDoubleJumped = false;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected bool CanJump()
    {
        return isGrounded || !hasDoubleJumped;
    }

    protected bool Jump()
    {
        bool jump = false;

        if (CanJump())
        {
            jump = true;
            if (!isGrounded)
                hasDoubleJumped = true;
            rb.velocity += Vector3.up * 20;
        }
        return jump;
    }

    protected void Move(float moveDirection)
    {
        rb.position += Vector3.right * (moveDirection * speed / 7);
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("solid"))
        {
            // Verification that we hit it on the top
            Ray collisionRay = new Ray(transform.position, collision.transform.position - transform.position);

            RaycastHit collisionRayHit;

            if (Physics.Raycast(collisionRay, out collisionRayHit))
            {
                if (collisionRayHit.normal == Vector3.up)
                {
                    isGrounded = true;
                    hasDoubleJumped = false;
                }
            }
        }
    }

    protected void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag.Contains("solid"))
        {
            isGrounded = false;
        }
    }
}

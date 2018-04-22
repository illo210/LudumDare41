using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    protected float speed = 1f;
    protected Rigidbody rb;
    protected bool isGrounded = false;
    protected bool hasDoubleJumped = false;
    protected int _health = 3;
    protected GameObject _healthBar;

    // Use this for initialization
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        GetHealthBar();
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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("solid"))
        {
            // Verification that we hit it on the top
            Vector3 direction = collision.transform.position - transform.position;
            direction = Vector3.Normalize(direction);
            if (direction.y < -0.1f)
            {
                isGrounded = true;
                hasDoubleJumped = false;
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

    protected virtual void GetHealthBar()
    {
        _healthBar = Instantiate(GameObject.Find("HealthBar"));
        UnityEngine.UI.Text text = _healthBar.GetComponent<UnityEngine.UI.Text>();
        text.text = new string('-', _health);
        text.enabled = true;
        FollowingEntity script = _healthBar.GetComponent<FollowingEntity>();
        script.followedEntity = transform;
        script.enabled = true;
        _healthBar.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    protected virtual void OnDestroy()
    {
        Destroy(_healthBar);
    }

    public virtual bool CanBeTarget(BaseProjectile projectile)
    {
        return true;
    }
}
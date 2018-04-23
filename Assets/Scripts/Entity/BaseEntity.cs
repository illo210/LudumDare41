using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    protected float speed = 1f;
    protected Rigidbody rb;
    protected bool isGrounded = false;
    protected bool hasDoubleJumped = false;
    public int _max_health;
    protected int _health;
    protected GameObject _healthBar;
    protected Animator _anim;
    protected Collider _collider;
    protected GameManagerBehavior gameManager;
    public bool _isAlive = true;
    protected bool _isInHighlight = false;
    protected Light _highlighter;
    protected SpriteRenderer _sprite;

    // Use this for initialization
    protected virtual void Start()
    {
        _health = _max_health;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehavior>();
        rb = GetComponent<Rigidbody>();
        GetHealthBar();
        _anim = gameObject.GetComponent<Animator>();
        _collider = GetComponent<Collider>();
        _highlighter = GetComponentInChildren<Light>();
        if (_highlighter)
            _highlighter.enabled = false;
        _sprite = GetComponent<SpriteRenderer>();
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
            rb.velocity = new Vector3(rb.velocity.x, 30, rb.velocity.z);
        }

        return jump;
    }

    public void ActiveHiglight()
    {
        _isInHighlight = true;
        if (_highlighter)
            _highlighter.enabled = _isInHighlight;
    }

    public void DeactiveHiglight()
    {
        _isInHighlight = false;
        if (_highlighter)
            _highlighter.enabled = false;
    }

    protected void Move(float moveDirection)
    {
        _anim.SetFloat("Move", moveDirection);
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

        if (collision.gameObject.tag.Contains("Entity"))
        {
            Physics.IgnoreCollision(collision.collider, _collider);
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
        text.text = new string('-', _health / 10);
        text.enabled = true;
        FollowingEntity script = _healthBar.GetComponent<FollowingEntity>();
        script.followedEntity = transform;
        script.enabled = true;
        _healthBar.transform.SetParent(GameObject.Find("Canvas").transform);
    }

    public void Decrease(int amount)
    {
        if (_health > amount)
        {
            _health -= amount;
            UnityEngine.UI.Text text = _healthBar.GetComponent<UnityEngine.UI.Text>();
            text.text = new string('-', _health / 10);
        }
        else
        {
            if (_isAlive)
            {
                _health = 0;
                gameManager.Gold += 1;
                Destroy(gameObject);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (_healthBar)
        {
            Destroy(_healthBar);
        }
    }

    public virtual bool CanBeTarget(BaseProjectile projectile)
    {
        return true;
    }
}
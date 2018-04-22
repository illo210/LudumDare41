using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseEntity
{
    [HideInInspector] public GameObject[] waypoints;
    protected int currentWaypoint = 0;
    protected Collider _collider;

    protected override void Start()
    {
        base.Start();
        speed *= 0.9f;
        _collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 1 
        Vector3 startPosition = waypoints[currentWaypoint].transform.position;
        Vector3 endPosition;
        if (waypoints.Length > currentWaypoint + 1)
        {
            endPosition = waypoints[currentWaypoint + 1].transform.position;
        }
        else
        {
            // No more waypoint
            return;
        }

        float deltaX = endPosition.x - transform.position.x;
        Move(deltaX / Mathf.Abs(deltaX));
        // 3 
        if (gameObject.transform.position.x < endPosition.x + 0.3f &&
            gameObject.transform.position.x > endPosition.x - 0.3f &&
            gameObject.transform.position.y < endPosition.y + 0.3f &&
            gameObject.transform.position.y > endPosition.y - 0.3f)
        {
            currentWaypoint++;
            if (currentWaypoint < waypoints.Length - 1)
            {
                // 3.a 
                // TODO: Rotate into move direction
                if (waypoints[currentWaypoint].tag == "Jumper")
                {
                    StartCoroutine(DoubleJump());
                }
            }
        }
    }

    private IEnumerator DoubleJump()
    {
        Jump();
        yield return new WaitForSeconds(0.1f);
        Jump();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.tag.Contains("Entity"))
        {
            Physics.IgnoreCollision(collision.collider, _collider);
        }

        if (collision.gameObject.tag.Contains("Base"))
        {
            Animator animDeath = gameObject.GetComponent<Animator>();
            StartCoroutine(Die(animDeath));
            //   AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            //    AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            // TODO: deduct health
        }
    }

    private IEnumerator Die(Animator animDeath)
    {
        Debug.Log("Deb");
        animDeath.SetBool("Explode", true);
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}
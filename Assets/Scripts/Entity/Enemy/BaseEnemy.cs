using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseEntity
{
    [HideInInspector] public GameObject[] waypoints;
    protected int currentWaypoint = 0;

    protected override void Awake()
    {
        base.Awake();
        speed *= 0.9f;
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

    private void RotateIntoMoveDirection()
    {
        //1
        Vector3 newStartPosition = waypoints[currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints[currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);
        //2
        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2(y, x) * 180 / Mathf.PI;
        //3
        GameObject sprite = gameObject.transform.Find("Sprite").gameObject;
        sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }

    private IEnumerator DoubleJump()
    {
        Jump();
        yield return new WaitForSeconds(0.1f);
        Jump();
        // RotateIntoMoveDirection();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);

        if (collision.gameObject.tag.Contains("Base"))
        {
            StartCoroutine(Die());
            if (gameManager.Health - 25 <= 0)
            {
                gameManager.gameOver = true;
                gameManager.Health = 0;
            }
            else
                gameManager.Health -= 25;
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
        }
    }

    
}
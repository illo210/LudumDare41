using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float distance = 10.0f;
    public float Speed = 1;

    void FixedUpdate()
    {
    
        float currentHeight = transform.position.y;
        float currentWidth = transform.position.x;
        transform.position = Vector3.Lerp(transform.position, target.position, Speed);
        transform.position -= Vector3.forward * distance;
        // Always look at the target
        transform.LookAt(target);
    }
}
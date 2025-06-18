using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Drag the player (circle sprite) here in the Inspector
    public float smoothSpeed = 0.125f;   // Controls the smoothing of the camera movement
    public Vector3 offset;        // Adjust this to set the camera's offset from the player

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}

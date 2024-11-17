using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private Transform target;

    private float fixedYPosition;
    private float initialXPosition;

    void Start()
    {
        fixedYPosition = transform.position.y;
        initialXPosition = transform.position.x; // Stores the initial X position
        // Set the initial position based on the target
        transform.position = new Vector3(target.position.x, fixedYPosition, target.position.z) + offset;
    }

    void FixedUpdate() 
    {
        Vector3 targetPosition = new Vector3(target.position.x, fixedYPosition, target.position.z) + offset;

        //  prevent camera moving left
        targetPosition.x = Mathf.Max(targetPosition.x, initialXPosition);

        //  move the camera
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.1f);
    }
}

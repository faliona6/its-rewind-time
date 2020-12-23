using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // Camera
    [SerializeField] Transform cameraParent;
    private Vector3 forward, right;

    // Movement
    [SerializeField] float moveSpeed;
    private Vector2 movementInput = Vector2.zero;
    private Vector3 direction;

    [SerializeField] float rotationSpeed;

    private void Start()
    {
        forward = cameraParent.forward;
        forward.y = 0;
        right = cameraParent.right;
    }

    private void FixedUpdate()
    {
        Vector3 direction = movementInput.x * right + movementInput.y * forward;
        direction = direction.normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        // Rotation
        Vector3 rotationDirection = Vector3.RotateTowards(transform.forward, direction, rotationSpeed, 0.0f);
        transform.rotation = Quaternion.LookRotation(rotationDirection);
    }

    /// <summary>
    /// Moves this GameObject based on a Horizontal and Vertical input Vector2
    /// </summary>
    public void Move(Vector2 movementInput)
    {
        this.movementInput = movementInput;
    }
}

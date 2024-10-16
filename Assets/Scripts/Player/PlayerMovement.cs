using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float deceleration;
    [SerializeField] private float rotationSpeed;

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();
    }

    void Update()
    {
        if (_player.IsDead && _player.CanMove) return;

        float inputVertical = Input.GetAxis("Vertical"); // W,S
        float inputHorizontal = Input.GetAxis("Horizontal"); // A,D

        bool isMoving = !Mathf.Approximately(inputVertical, 0f) || !Mathf.Approximately(inputHorizontal, 0f);

        Vector3 verticalMovement = Vector3.forward * inputVertical;
        Vector3 horizontalMovement = Vector3.right * inputHorizontal;

        Vector3 movementDirection = (verticalMovement + horizontalMovement).normalized;

        Vector3 desiredVelocity = movementDirection * maxSpeed;

        if (isMoving)
        {
            Vector3 force = (desiredVelocity - rb.velocity).normalized * acceleration;
            rb.AddForce(force);
        }
        else if (rb.velocity.magnitude > 0)
        {
            rb.velocity /= deceleration;
        }

        animator.SetFloat("Speed", rb.velocity.magnitude);

        //rotation
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerActions : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 _movementVector;
    private CharacterController _characterController;
    private PlayerInput _playerInput;
    private NavMeshHit hit;
    private float gravity = 5;
    private float _velocityY = 0;
    [SerializeField]
    private float movementSpeed;
    
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bool groundedPlayer = _characterController.isGrounded;
        if (groundedPlayer)
        {
            _velocityY = 0;
        }
        else
        {
            _velocityY -= gravity * Time.deltaTime;
        }
    }

    bool IsNextPositionValid(Vector3 newPosition)
    {
        return NavMesh.SamplePosition(transform.position + new Vector3(newPosition.x, 0, newPosition.z), out hit, 1.1f, NavMesh.AllAreas);
    }

    public void HandleMovement(Vector3 movementVector3)
    {
        Vector3 nextPosition = Vector3.zero;
        
        if (movementVector3.magnitude > 0)
        {
            movementVector3.y = _velocityY;
            nextPosition = movementVector3 * (Time.deltaTime * movementSpeed);
        }
        else
        {
            nextPosition.y = _velocityY;
        }

        if(IsNextPositionValid(nextPosition))
            _characterController.Move(nextPosition);
    }
}

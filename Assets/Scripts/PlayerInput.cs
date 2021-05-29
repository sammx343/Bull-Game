using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour, IInput
{
  // Start is called before the first frame update
  private Vector3 _movementVector;
  private new Camera _camera;
  private RaycastHit _hit;
  private GameObject _mouseSelector;
  public GameObject mouseSelectorPrefab;

  public UnityAction<Vector3> OnPlayerMovementInput { get; set; }
  public UnityAction<Vector3> OnPlayerClick { get; set; }

  private void Start()
  {
    _camera = Camera.main;
    _mouseSelector = Instantiate(mouseSelectorPrefab);
    _mouseSelector.SetActive(false);
  }

  void Update()
  {
    HandleMovement();
    HandleMouseClick();
    HandleMouseMovement();
  }
  
  private void LateUpdate()
  {
    _mouseSelector.transform.position = new Vector3(_hit.point.x, 0.1f, _hit.point.z);
  }

  private void HandleMouseClick()
  {
    if (Input.GetMouseButtonDown(0))
    {
      OnPlayerClick?.Invoke(_hit.point);
    }
  }

  private void HandleMouseMovement()
  {
    Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
    {
      Debug.DrawRay(ray.origin, ray.direction * 1000, Color.white);
      _mouseSelector.SetActive(true);
    }
    else
    {
      Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
      _mouseSelector.SetActive(false);
    }
  }

  void HandleMovement()
  {
    _movementVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

    if (_movementVector.magnitude > 0.1f)
      OnPlayerMovementInput?.Invoke(_movementVector.normalized);
  }
}



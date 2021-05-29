using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IInput
{
  UnityAction<Vector3> OnPlayerMovementInput { get; set; }
  UnityAction<Vector3> OnPlayerClick { get; set; }
}

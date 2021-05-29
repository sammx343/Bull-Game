using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlayerMovementHandler", order = 1)]
public class PlayerMovementHandler : ScriptableObject
{
    public UnityAction<Vector3> movementAction;
}

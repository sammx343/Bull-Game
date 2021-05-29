using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameManager", menuName = "Ball-Prototype/GameManager", order = 0)]
public class GameManager : ScriptableObject
{
  public UnityAction damageChange;
}

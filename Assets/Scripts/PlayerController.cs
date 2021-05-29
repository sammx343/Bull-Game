using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using AI_States;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{

  private IInput _input;
  private PlayerActions _playerActions;
  public PlayerBall _playerBall;
  public PlayerDead _playerDead;
  private BoxCollider _collider;
  private CharacterController _characterController;
  [SerializeField] private int _playerPoints;
  public bool playerWonLastRound;
  public bool disabledCharacterController;

  public int PlayerPoints
  {
    get => _playerPoints;
    set => _playerPoints = value;
  }

  public int PlayerIdentifier { get; private set; }

  private void Awake()
  {
    PlayerIdentifier = GetInstanceID();
  }
  
  private void OnEnable()
  {
    _input = GetComponent<IInput>();
    _playerActions = GetComponent<PlayerActions>();
    _characterController = GetComponent<CharacterController>();
    _collider = GetComponent<BoxCollider>();

    if (TryGetComponent(out PlayerDead playerDead))
    {
      _playerDead = playerDead;
      _playerDead.playerDies += PlayerDiesAndDisappear;
    }

    PlayerAppearsOrRestarts();
  }
  
  private void Update()
  {
    _characterController.detectCollisions = disabledCharacterController;
  }

  private void OnDisable()
  {
    DisablePlayerInputs();
  }

  private void OnDestroy()
  {
    DisablePlayerInputs();
  }

  private void PlayerDiesAndDisappear()
  {
    playerWonLastRound = false;
    if (_playerBall != null)
      _playerBall.PlayerDied();

    DisablePlayerInputs();
    EnableDisablePlayerMovement(false);
  }

  public void EnableDisablePlayerMovement(bool shouldEnable)
  {
    _collider.isTrigger = shouldEnable;
    _collider.enabled = shouldEnable;
    _characterController.enabled = shouldEnable;
  }

  public void PlayerAppearsOrRestarts()
  {
    _playerDead.PlayerRestart();
    transform.rotation = Quaternion.identity;
    
    _input.OnPlayerMovementInput += _playerActions.HandleMovement;
    
    if (TryGetComponent(out PlayerBall playerBall))
    {
      _playerBall = playerBall;
      _input.OnPlayerClick += _playerBall.ThrowBall;
    }

    EnableDisablePlayerMovement(true);
  }
  
  private void DisablePlayerInputs()
  {
    if (_input.OnPlayerMovementInput != null)
    {
      _input.OnPlayerMovementInput -= _playerActions.HandleMovement;
    }

    if (_input.OnPlayerClick != null)
    {
      _input.OnPlayerClick -= _playerBall.ThrowBall;
    }
  }
}

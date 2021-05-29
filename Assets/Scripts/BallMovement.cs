using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallMovement : MonoBehaviour
{
  // Start is called before the first frame 
  [SerializeField] private float timeToReachTarget = 1f;
  [SerializeField] public PlayerBall playerBall;
  [SerializeField] private float _timer;
  [SerializeField] private bool ballWasThrow;
  [SerializeField] private bool ballIsOnPlayer;
  private Vector3 _ballStartingPosition;
  public UnityAction<GameObject> ballThrow;
  private void Awake()
  {
    _ballStartingPosition = Vector3.zero;
    ballIsOnPlayer = false;
    BallIsStatic();
  }

  void BallIsStatic()
  {
    _timer = 0;
    ballWasThrow = false;
  }

  void BallReachedPlayer()
  {
    ballIsOnPlayer = true;
    playerBall.BallRecieved(gameObject);
  }

  public void CurrentPlayerDied(int delayToThrow)
  {
    ballIsOnPlayer = false;
    playerBall = null;
    ChooseRandomPlayerToThrowBall(delayToThrow);
  }

  public bool ChooseRandomPlayerToThrowBall(int delayToThrow = 0)
  {
    PlayerBall[] _playerBalls = FindObjectsOfType<PlayerBall>();
    if (_playerBalls.Length <= 1) return false;
    
    PlayerBall playerBall;
    int steps = 0;
    do
    {
      int randomPlayerNumber = UnityEngine.Random.Range(0, _playerBalls.Length);
      playerBall = _playerBalls[randomPlayerNumber];
      steps++;
    }
    while (!playerBall.CanRecieveBall && steps < 20);

    StartCoroutine(ThrowWithDelay(delayToThrow, playerBall));

    return true;
  }

  IEnumerator ThrowWithDelay(int delay, PlayerBall playerBall)
  {
    yield return new WaitForSeconds(delay);
    BallThrowToAnotherPlayer(playerBall);
  }

  public void BallThrowToAnotherPlayer(PlayerBall playerBall)
  {
    _ballStartingPosition = transform.position;
    this.playerBall = playerBall;
    ballWasThrow = true;
    ballIsOnPlayer = false;
    ballThrow?.Invoke(gameObject);
  }

  void Update()
  {
    if (ballWasThrow)
    {
      if (_timer < timeToReachTarget)
      {
        Vector3 playerPosition = playerBall.transform.position;

        transform.position =
            Vector3.Lerp(_ballStartingPosition, new Vector3(playerPosition.x, 2, playerPosition.z), _timer / timeToReachTarget);

        _timer += Time.deltaTime;

      }
      else
      {
        BallReachedPlayer();
        BallIsStatic();
      }
    }

    if (ballIsOnPlayer)
    {
      Vector3 playerPosition = playerBall.transform.position;
      transform.position = new Vector3(playerPosition.x, 2, playerPosition.z);
    }
  }
}

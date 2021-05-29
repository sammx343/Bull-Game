using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIThrowBall : MonoBehaviour
{
  [SerializeField] [Range(2, 6)] private float maxTimeToThrowBall;
  private float _randomTimeToThrow;
  private float _timer;
  private PlayerBall _playerBall;
  private PlayerAiInput _aiInput;
  private PlayerBall[] _playerBalls;
  void Start()
  {
    _playerBall = GetComponent<PlayerBall>();
    _aiInput = GetComponent<PlayerAiInput>();
    _playerBall._ballRecieved += StartTimerToThrow;
  }

  // Update is called once per frame
  void Update()
  {
    if (_playerBall.PlayerHasBall)
    {
      if (_timer < _randomTimeToThrow)
      {
        _timer += Time.deltaTime;
      }
      else
      {
        ThrowBall();
      }
    }
  }

  void ThrowBall()
  {
    _playerBalls = FindObjectsOfType<PlayerBall>();
    int playerBallsThatCanRecieveBall = _playerBalls.Length;
    
    foreach (var playerBall in _playerBalls)
    {
      if (!playerBall.CanRecieveBall) playerBallsThatCanRecieveBall--;
    }
    
    if(playerBallsThatCanRecieveBall <= 1) return;

    int random = Random.Range(0, _playerBalls.Length);
    
    if(!_playerBalls[random].CanRecieveBall) return;

    _aiInput.OnPlayerClick?.Invoke(_playerBalls[random].transform.position);
  }

  private void StartTimerToThrow()
  {
    _timer = 0;
    _randomTimeToThrow = Random.Range(0, maxTimeToThrowBall);
  }
}

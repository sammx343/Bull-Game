using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDead : MonoBehaviour
{
  private Animator _animator;
  private PlayerBall _playerBall;
  public bool IsPlayerDead;
  public UnityAction playerDies;
  private static readonly int Dead = Animator.StringToHash("PlayerDead");

  void Start()
  {
    _animator = GetComponent<Animator>();
    _playerBall = GetComponent<PlayerBall>();
  }

  private void OnTriggerEnter(Collider other)
  {
    BullDetection(other.gameObject);
  }

  private void BullDetection(GameObject other)
  {
    if (IsPlayerDead) return;
    
    if (other.CompareTag("Bull"))
    {
      if (_playerBall != null && _playerBall.PlayerHasBall)
      {
        IsPlayerDead = true;
        
        PlayerDies();

        if (_animator != null)
        {
          PlayDeadAnimation();
        }
      }
    }
  }

  public void PlayerRestart()
  {
    if(_animator != null) _animator.SetBool(Dead, false);
    IsPlayerDead = false;
  }

  private void PlayerDies()
  {
    InformBallPlayerDied();
    playerDies?.Invoke();
  }

  private void PlayDeadAnimation()
  {
    _animator.SetBool(Dead, true);
  }

  private void InformBallPlayerDied()
  {
    if (_playerBall != null)
    {
      _playerBall.CanRecieveBall = false;
      _playerBall.ball.GetComponent<BallMovement>().CurrentPlayerDied(2);
    }
  }

  private void PlayerDeadAnimationEnd()
  {
    // Destroy(gameObject);
  }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBall : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField] private LayerMask layerMask;
  public bool PlayerHasBall { get; set; }
  public bool CanRecieveBall { get; set; } = true;
  public GameObject ball;
  public UnityAction _ballRecieved;
  [SerializeField] private float holdBallTimer;

  public void BallRecieved(GameObject ball)
  {
    this.ball = ball;
    PlayerHasBall = true;
    _ballRecieved?.Invoke();
  }

  public void PlayerDied()
  {
    BallThrowed();
  }

  private void BallThrowed()
  {
    PlayerHasBall = false;
    ball = null;
  }

  public void ThrowBall(Vector3 mousePosition)
  {
    if (!PlayerHasBall) return;

    Vector3 mousePositionBase = new Vector3(mousePosition.x, 0, mousePosition.z);
    Vector3 mousePositionTop = new Vector3(mousePosition.x, 2, mousePosition.z);

    Collider[] colliders = Physics.OverlapCapsule(mousePositionBase, mousePositionTop, 3, layerMask);

    colliders = colliders
                        .Where(collider => collider.transform.GetInstanceID() != transform.GetInstanceID())
                        .OrderBy(collider =>
                            Vector3.Distance(collider.transform.position, mousePositionBase)
                            )
                        .ToArray();

    if (colliders.Length < 1) return;

    if (colliders[0].TryGetComponent(out PlayerBall playerBall))
    {
      if(!playerBall.CanRecieveBall) return;
      
      ball.GetComponent<BallMovement>().BallThrowToAnotherPlayer(playerBall);
      BallThrowed();
    }
  }
}




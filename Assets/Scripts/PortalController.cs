using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalController : MonoBehaviour
{
  // Start is called before the first frame update
  [SerializeField] private GameObject clock1, clock2;
  [SerializeField] private float clockRotationTime;
  private Quaternion _quaternionOriginalRotation;
  private Animator _animator;
  private bool _startClockRotation;
  private float _clockRotationTimer;
  [SerializeField]
  private Transform portalPoint;
  public Transform PortalPoint => portalPoint;
  public UnityAction endClockRotation;
  
  void Start()
  {
    _animator = GetComponent<Animator>();

    _clockRotationTimer = 0;
    _quaternionOriginalRotation = clock1.transform.rotation;
  }

  // Update is called once per frame
  void Update()
  {
    if (_startClockRotation)
    {
      if (_clockRotationTimer < clockRotationTime)
      {
        RotateClock(clock1, Quaternion.Euler(0, 0, 180));
        RotateClock(clock2, Quaternion.Euler(0, 0, -180));
        _clockRotationTimer += Time.deltaTime;
      }
      else
      {
        _startClockRotation = false;
        EndClockRotation();
        StartCoroutine(ClosePortal());
      }
    }
  }

  public void RestartPortal()
  {
    _clockRotationTimer = 0;
    _animator.Play("Portal-Opens");
    clock1.transform.rotation = Quaternion.identity;
    clock2.transform.rotation = Quaternion.identity;
  }

  IEnumerator ClosePortal()
  {
    yield return new WaitForSeconds(2);
    _animator.Play("Portal-Closes");
  }

  void RotateClock(GameObject clock, Quaternion quaternion)
  {
    clock.transform.rotation = Quaternion.Lerp(_quaternionOriginalRotation, quaternion, _clockRotationTimer / clockRotationTime);
  }

  void EndClockRotation()
  {
    endClockRotation?.Invoke();
  }

  void StartClockRotation()
  {
    _startClockRotation = true;
  }
}
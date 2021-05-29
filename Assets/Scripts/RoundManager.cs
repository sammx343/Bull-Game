using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AI_States;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject bullPrefab;
    private BullController _bullController;
    private BallMovement _ballMovement;
    
    [SerializeField] private float timeToCreateBull = 2;
    private PortalController _portalController;
    public PlayerController[] PlayerControllers { get; private set; }
    private bool _isbullPrefabNull;
    private bool _isballPrefabNull;
    
    private GameObject[] Players { get; set; }
    private SceneManager _sceneManager;
    private float radius = 10;
    
    public int distanceFromCenter;
    public UnityAction<PlayerController> roundEnded;
    public UnityAction<PlayerController[]> roundStarts;
    private PlayerController _playerLastRoundWinner;

    void Start()
    {
        Cursor.visible = false;
        _portalController = FindObjectOfType<PortalController>();
        PlayerControllers = FindObjectsOfType<PlayerController>();
        _sceneManager = FindObjectOfType<SceneManager>();
        _portalController.endClockRotation += CreateBall;

        _isbullPrefabNull = bullPrefab == null;
        _isballPrefabNull = ballPrefab == null;
        
        CreatePlayersInScene(PlayerControllers);
        AddDeadPlayerListener(PlayerControllers);
        
        roundStarts?.Invoke(PlayerControllers);
    }

    public void RestartRound()
    {
        _playerLastRoundWinner.EnableDisablePlayerMovement(false);
        
        //This goes here because the all the players character controller must be disabled to change the transform 
        CreatePlayersInScene(PlayerControllers);
        
        foreach (var playerController in PlayerControllers)
        {
            playerController._playerBall.CanRecieveBall = true;
            if (!playerController.playerWonLastRound)
            {
                playerController.PlayerAppearsOrRestarts();
            }
        }
        
        _playerLastRoundWinner.EnableDisablePlayerMovement(true);

        _portalController.RestartPortal();
        _ballMovement.playerBall.PlayerDied();
        Destroy(_ballMovement.gameObject);
        Destroy(_bullController.gameObject);
        
        roundStarts?.Invoke(PlayerControllers);
    }

    void CreatePlayersInScene(PlayerController[] players)
    {
        for (int index = 0; index < players.Length; index++)
        {
            players[index].transform.position = new Vector3(radius * Mathf.Cos( 2*Mathf.PI * index/players.Length), 1, radius* Mathf.Sin(2*Mathf.PI  * index/players.Length));
        }
    }

    void AddDeadPlayerListener(PlayerController[] players)
    {
        foreach (var player in players)
        {
            player._playerDead.playerDies += PlayerDies;
        }
    }

    void PlayerDies()
    {
        int alivePlayersLength = PlayerControllers.Length;
        foreach (var player in _sceneManager.allPlayers)
        {
            if(player.GetComponent<PlayerDead>().IsPlayerDead) alivePlayersLength--;
        }

        RoundEnds(alivePlayersLength);
    }

    void RoundEnds(int alivePlayersLength)
    {
        if (alivePlayersLength <= 1)
        {
            PlayerController playerController = GetLastPlayer();
            _playerLastRoundWinner = playerController;
            
            roundEnded?.Invoke(playerController);
        }
    }
    
    PlayerController GetLastPlayer()
    {
        return PlayerControllers.First(controller => controller._playerDead.IsPlayerDead == false);
    }
    
    void CreateBall()
    {
        if (_isballPrefabNull) return;
    
        _ballMovement = Instantiate(ballPrefab, _portalController.PortalPoint.position, Quaternion.identity).GetComponent<BallMovement>();
        _ballMovement.ChooseRandomPlayerToThrowBall(0);

        if(_isbullPrefabNull) return;
        StartCoroutine(CreateBull(_ballMovement));
    }

    IEnumerator CreateBull(BallMovement ballMovement)
    {
        yield return new WaitForSeconds(timeToCreateBull);
    
        _bullController = Instantiate(bullPrefab, new Vector3(0, -3, 0), Quaternion.identity).GetComponent<BullController>();
        _bullController.FollowBall(ballMovement.gameObject);
    }
}

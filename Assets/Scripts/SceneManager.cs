using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
  [Range(2,8)] 
  public int numberOfPlayers = 2;
  private bool _isbullPrefabNull;
  private bool _isballPrefabNull;
  private RoundManager _roundManager;
  [Range(2,8)] [SerializeField]
  private float nextRoundWait = 2;
  public GameObject playerPrefab;
  public GameObject playerAiPrefab;
  public List<GameObject> allPlayers;
  
  [Range(2,8)] public  int maxRoundsWins = 3;
  void Awake()
  {
    _roundManager = FindObjectOfType<RoundManager>();
    
    InstantiatePlayers(playerPrefab, 1, 1);
    InstantiatePlayers(playerAiPrefab, numberOfPlayers - 1, 2);

    if(_roundManager != null)
      _roundManager.roundEnded += RoundEnded;
  }

  void InstantiatePlayers(GameObject playerPrefab, int number, int nameOffset)
  {
    for (int i = 0; i < number; i++)
    {
      GameObject player = Instantiate(playerPrefab);
      player.name = "Player " + (i + nameOffset);
      allPlayers.Add(player);
    }
  }

  void RoundEnded(PlayerController playerController)
  {
    playerController.PlayerPoints++;
    playerController.playerWonLastRound = true;
    StartCoroutine(RestartRoundAfterSeconds());
  }

  IEnumerator RestartRoundAfterSeconds()
  {
    yield return new WaitForSeconds(nextRoundWait);
    _roundManager.RestartRound();
  }
}

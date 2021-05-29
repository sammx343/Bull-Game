using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIRoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject panelPlayerScore;
    [SerializeField] private GameObject panelRound;
    private Canvas _canvas;
    private GameObject _panelRound;
    private RoundManager _roundManager;
    private float _panelRoundHeight = 0;

    List<GameObject> _playerScores = new List<GameObject>();

    void Start()
    {
        _canvas = FindObjectsOfType<Canvas>().First( _canvas => _canvas.name == "MainCanvas" );
        _roundManager = FindObjectOfType<RoundManager>();
        _roundManager.roundStarts += CreatePlayerScore;
        
        if (panelRound != null)
        {
            _panelRound = Instantiate(panelRound, _canvas.transform);
            _panelRoundHeight = _panelRound.GetComponent<RectTransform>().rect.height;
        }
    }

    void CreatePlayerScore(PlayerController[] playerControllers)
    {
        DestroyAndClearScores();
        playerControllers = playerControllers
            .OrderBy( playerController => playerController.PlayerPoints)
            .Reverse()
            .ToArray();
        
        for (int index = 0; index < playerControllers.Length; index++)
        {
            GameObject playerScoreUi = Instantiate(panelPlayerScore, _panelRound.transform);
            
            Text playerName = GetTextComponentFromChildren(playerScoreUi, "PlayerName");
            Text points = GetTextComponentFromChildren(playerScoreUi, "Points");
            
            playerName.text = playerControllers[index].name;
            points.text = playerControllers[index].PlayerPoints + "";
            
            float playerScoreUIHeight = PlayerScorePositionInsidePanel(playerScoreUi, index);
            
            _playerScores.Add(playerScoreUi);

            ChangeMainPanelHeight(playerScoreUIHeight, index + 1);
        }
    }

    Text GetTextComponentFromChildren(GameObject playerScoreUi, string childrenName)
    {
        return playerScoreUi.GetComponentsInChildren<Transform>().
        First(child => child.name == childrenName)
        .GetComponent<Text>();
    }

    void DestroyAndClearScores()
    {
        foreach (var playerScore in _playerScores)
        {
            Destroy(playerScore);
        }
        _playerScores.Clear();
    }

    void ChangeMainPanelHeight(float playerScoreUIHeight, float index)
    {
        RectTransform parentPanelRect = _panelRound.GetComponent<RectTransform>();
        parentPanelRect.sizeDelta = new Vector2(parentPanelRect.rect.width, _panelRoundHeight  + playerScoreUIHeight * index );
    }

    float PlayerScorePositionInsidePanel(GameObject playerScoreUi, int index)
    {
        RectTransform rectTransform = playerScoreUi.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, -(_panelRoundHeight + rectTransform.rect.height * index)); ;
        return rectTransform.rect.height; 
    }
}

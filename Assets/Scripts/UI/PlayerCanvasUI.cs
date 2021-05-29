using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Camera _camera;
    private Transform player;
    private RectTransform _rectTransform;
    private float _rotation;
    
    void Start()
    {
        Text playerName = GetComponentInChildren<Text>();
        player = transform.root;
        playerName.text = player.name;
        
        _rectTransform = GetComponent<RectTransform>();
        
        _camera = Camera.main;
        _rotation = _camera.transform.rotation.eulerAngles.x;
    }

    // Update is called once per frame
    void Update()
    {
        _rectTransform.rotation = Quaternion.Euler(new Vector3(_rotation,0,0));
    }
}

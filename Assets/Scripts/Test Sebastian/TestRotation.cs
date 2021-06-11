using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    Transform pointer;
    private float angle;
    public float speed = 5;
    private Vector3 Vec;
    public float turnFactor = 5;
    private TopDownCarController _topDownCarController;
    void Start()
    {
        angle = 0;
        Vec = Vector3.zero;
        _topDownCarController = GetComponent<TopDownCarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit _hit;
        if (Physics.Raycast(ray, out _hit, Mathf.Infinity))
        {
            pointer.transform.position = new Vector3(_hit.point.x, 0.1f, _hit.point.z);
        }

        Vector3 direction = pointer.transform.position - transform.position;
        angle = Mathf.LerpAngle(angle, Vector3.SignedAngle(direction.normalized, transform.forward, Vector3.up), Time.deltaTime * speed)%360;
        Debug.Log(-angle);
        //
        // transform.rotation =  Quaternion.Euler(0,angle, 0);
         
        // Vec.x = Input.GetAxis("Horizontal");  
        Vec.x = (-angle/360) * turnFactor; 
        Vec.z = 1;  
        _topDownCarController.SetInputVector(new Vector2(Vec.x, Vec.z));
    }
}

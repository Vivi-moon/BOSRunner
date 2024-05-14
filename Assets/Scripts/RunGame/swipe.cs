using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swipe : MonoBehaviour
{

    [SerializeField] private float _speedwalk;

    private CharacterController _characterController;
    private Vector3 _walkDirection;
    private Vector3 _test;
    public RoadGenerator _roadGenerator;
    

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }


    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        
        
        _walkDirection = transform.right * x;
       
        
    }

    private void FixedUpdate()
    {
        
        if (_roadGenerator.GameActive && transform.position.x <= 2.5 && transform.position.x >= -2.5)
        {
            Walk(_walkDirection);
        }
        else
        {
            if (transform.position.x > 0)
            {
                transform.position = new Vector3(2.5f, 1, 0);
            }
            else
            {
                transform.position = new Vector3(-2.5f, 1, 0);
            }
            
        }
        //Walk(_walkDirection);
    }

    private void Walk(Vector3 direction)
    {
        
        _characterController.Move(direction * _speedwalk * Time.fixedDeltaTime);
    }
}

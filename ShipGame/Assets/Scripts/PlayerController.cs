using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float currentSpeed;
    
    public float CurrentSpeed => currentSpeed;
    
    private Vector3 lastPosition;


    // Start is called before the first frame update
    void Start()
    {
        lastPosition = gameObject.transform.position;
    }

    private void Update()
    {
        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        var position = gameObject.transform.position;
        currentSpeed = (position - lastPosition).magnitude / Time.deltaTime;

        lastPosition = position;
    }
}

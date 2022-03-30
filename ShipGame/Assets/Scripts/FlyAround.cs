using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class FlyAround : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
   
    

    // Update is called once per frame
    private void Update()
    {
        // Rotates camera looking at target
        transform.LookAt(target.transform);
        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }
    
}
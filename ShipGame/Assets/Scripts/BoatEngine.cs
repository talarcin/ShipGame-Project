using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngine : MonoBehaviour
{
    public Transform engineTransform;
    public float powerFactor;
    public float maxPower;
    public float currentPower;
    private Rigidbody boatRb;
    private float engineRotation;
    
    public float MaxPower => maxPower;
    public float CurrentPower => currentPower;


    // Start is called before the first frame update
    void Start()
    {
        boatRb = GetComponentInParent<Rigidbody>();
    }

    public void CalculateForce(float scaling)
    {
        currentPower += scaling * powerFactor;
    }

    public void Thrust()
    {
        Vector3 forceToAdd = -engineTransform.forward * currentPower;

        float waveYPos = WaterController.instance.GetWaveYPos(engineTransform.position, Time.time);

        if (engineTransform.position.y < waveYPos)
        {
            boatRb.AddForceAtPosition(forceToAdd, engineTransform.position, ForceMode.Force);
        }
        else
        {
            boatRb.AddForceAtPosition(Vector3.zero, engineTransform.position);
        }
    }
}

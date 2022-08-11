using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatEngine : MonoBehaviour
{
    public float powerFactor;
    public float maxPower;
    public float maxSpeed = 50f;
    public float currentPower;
    private float maxRotationAngle = 45f;
    
    private Vector3 engineRotation = new Vector3(0f, 0f, 0f);
    
    private Rigidbody boatRb;
    private PlayerController playerController;
    public Transform engineTransform;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        boatRb = GetComponentInParent<Rigidbody>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void FixedUpdate()
    {
        float turnInput = Input.GetAxis("Horizontal");
        RotateEngine(turnInput);
        
        float forwardInput = Input.GetAxis("Vertical");

        if (forwardInput > 0f)
        {
            if (playerController.CurrentSpeed < maxSpeed && currentPower < maxPower)
            {
                CalculateCurrentPower(forwardInput);
            }
        }
        else
        {
            currentPower = 0f;
        }
        
        Thrust();
    }

    public void CalculateCurrentPower(float scaling)
    {
        currentPower += scaling * powerFactor;
    }

    public void Thrust()
    {
        Vector3 forceToAdd = -engineTransform.right * currentPower;

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

    public void RotateEngine(float factor)
    {
        engineRotation.y = -factor * 45f;
        engineTransform.localEulerAngles = engineRotation;
    }
}

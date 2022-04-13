using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public static WaterController instance;

    public bool isMoving;

    // sets wave height and distance
    public float scale = 0.1f;
    public float speed = 1.0f;
    // sets width between waves
    public float waveDistance = 1f;
    // noise parameters
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public float GetWaveYPos(Vector3 position, float timeSinceStart)
    {
        if (isMoving)
        {
            return WaveTypes.SinXWave(position, speed, scale, waveDistance, noiseStrength, noiseWalk, timeSinceStart);
        }
        else
        {
            return 0f;
        }
    }

    public float DistanceToWater(Vector3 position, float timeSinceStart)
    {
        float yPos = position.y;

        float waterYPos = GetWaveYPos(position, timeSinceStart);

        float distance = yPos - waterYPos;

        return distance;
    }

    
}

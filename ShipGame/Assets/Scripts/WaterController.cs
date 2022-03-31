using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    public static WaterController instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float GetWaveYPos(Vector3 position, float timeSinceStart)
    {
        return 0f;
    }

    public float DistanceToWater(Vector3 position, float timeSinceStart)
    {
        float yPos = position.y;

        float waterYPos = GetWaveYPos(position, timeSinceStart);

        float distance = yPos - waterYPos;

        return distance;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EndlessWaterSquare : MonoBehaviour
{
    public GameObject boatObj;

    public GameObject waterSquareObj;

    private float squareWidth = 800f;
    private float innerSquareRes = 5f;

    private float seconsSinceStart;
    private Vector3 boatPos;
    private Vector3 oceanPos;

    private bool hasThreadUpdatedWater;


    // Start is called before the first frame update
    void Start()
    {
        CreateWaterPlane();

        seconsSinceStart = Time.time;

        ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateWaterWithThreadPooling));

        StartCoroutine(UpdateWater());

    }

    IEnumerator UpdateWater()
    {
        while (true)
        {
            if (hasThreadUpdatedWater)
            {
                transform.position = oceanPos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        seconsSinceStart = Time.time;

        boatPos = boatObj.transform.position;
    }
    
    private void CreateWaterPlane()
    {
        throw new System.NotImplementedException();
    }

    void UpdateWaterWithThreadPooling(object state)
    {
        throw new System.NotImplementedException();
    }
}

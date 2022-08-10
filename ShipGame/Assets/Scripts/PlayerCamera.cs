using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerGameObject;

    private Transform playerPosition;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = playerGameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float torqueForce = 1000f;
    private Rigidbody playerRb;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float turnValue = Input.GetAxis("Horizontal");
        playerRb.AddTorque(Vector3.back * (torqueForce * turnValue), ForceMode.Force);
    }
}

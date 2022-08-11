using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    BoatEngine boatEngine;
    private Rigidbody playerRb;

    public float currentSpeed;
    private float maxSpeed = 50f;

    private Vector3 lastPosition;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        boatEngine = GetComponentInChildren<BoatEngine>();

        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float forwardInput = Input.GetAxis("Vertical");
        
        if (boatEngine.CurrentPower < boatEngine.MaxPower && currentSpeed < maxSpeed)
        {
            boatEngine.CalculateForce(forwardInput);
            boatEngine.Thrust();
        }

        CalculateSpeed();

    }

    private void CalculateSpeed()
    {
        var position = transform.position;
        currentSpeed = (position - lastPosition).magnitude / Time.time;

        lastPosition = position;
    }
}

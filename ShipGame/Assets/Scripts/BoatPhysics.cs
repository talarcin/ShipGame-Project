using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BoatPhysics : MonoBehaviour
{
    public GameObject underWaterObject;

    private Mesh underWaterMesh;
    private float waterDensity = 1027f;
    private ModifyBoatMesh modifyBoatMesh;
    private Rigidbody boatRb;

    // Start is called before the first frame update
    private void Start()
    {
        // Get boat's RigidBody
        boatRb = gameObject.GetComponent<Rigidbody>();
        
        // Initialization of script that will modify boat mesh
        modifyBoatMesh = new ModifyBoatMesh(gameObject);

        // Meshes that are below and above the water
        underWaterMesh = underWaterObject.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    private void Update()
    {
        // generate underwater mesh
        modifyBoatMesh.GenerateUnderWaterMesh();

        // Display under water mesh
        modifyBoatMesh.DisplayMesh(underWaterMesh, "UnderWaterMesh", modifyBoatMesh.underWaterTriangleData);
    }

    private void FixedUpdate()
    {
        // add forces to underwater boat parts
        if (modifyBoatMesh.underWaterTriangleData.Count > 0)
        {
            AddUnderWaterForces();
        }
    }

    // adds forces to surfaces underwater
    private void AddUnderWaterForces()
    {
        List<TriangleData> underWaterTriangles = modifyBoatMesh.underWaterTriangleData;

        // iterate over all underwater triangles and add buoyancy force
        for (int i = 0; i < underWaterTriangles.Count; i++)
        {
            TriangleData triangle = underWaterTriangles[i];

            Vector3 buoyancyForce = CalculateBuoyancyForce(waterDensity, triangle);

            boatRb.AddForceAtPosition(buoyancyForce, triangle.triangleCenter);


            // for debugging

            // display triangle normal vector
            Debug.DrawRay(triangle.triangleCenter, triangle.triangleNormalVect * 3f, Color.white);

            // display force vector
            Debug.DrawRay(triangle.triangleCenter, buoyancyForce.normalized * -3f, Color.blue);
        }
    }

    private Vector3 CalculateBuoyancyForce(float density, TriangleData triangle)
    {
        /* calculate buoyancy force according to:
         * force_buoyancy = density * g * V with
         * g - gravitational acceleration
         * V- volume of fluid directly above curved surface
         *
         * V = z * S * n with
         * z - distance to surface
         * S - surface area
         * n - normal to the surface
         */

        Vector3 buoyancyForce = density * Physics.gravity.y * triangle.distFromTriangleCenterToSurface *
                                triangle.triangleArea * triangle.triangleNormalVect;
        
        // horitontal forces cancel out at the end

        buoyancyForce.x = 0f;
        buoyancyForce.z = 0f;

        return buoyancyForce;
    }
}
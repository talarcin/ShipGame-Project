using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BoatPhysics : MonoBehaviour
{
    [SerializeField] private GameObject underWaterObject;

    private Mesh underWaterMesh;
    private float waterDensity = 1027f;
    private ModifyBoatMesh modifyBoatMesh;
    private Rigidbody boatRB;
    
    // Start is called before the first frame update
    private void Start()
    {
        // Get boat's RigidBody
        boatRB = gameObject.GetComponent<Rigidbody>();
        
        // not new ModifyBoatMesh(gameObject) anymore?
        // Initialization of script that will modify boat mesh
        modifyBoatMesh = gameObject.AddComponent<ModifyBoatMesh>();
        
        // Meshes that are below and above the water
        underWaterMesh = underWaterObject.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    private void Update()
    {
        // generate underwater mesh
        modifyBoatMesh.GenerateUnderWaterMesh();

        // Display under water mesh
        modifyBoatMesh.DisplayMesh(underWaterMesh, "UnderWater Mesh", modifyBoatMesh.underWaterTriangleData);
    }
}
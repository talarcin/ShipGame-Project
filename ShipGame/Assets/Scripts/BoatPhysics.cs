using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class BoatPhysics : MonoBehaviour
{
    public GameObject boatMeshObject;
    public GameObject underWaterObject;
    public GameObject aboveWaterObject;

    public Vector3 centerOfMass;
    
    private ModifyBoatMesh modifyBoatMesh;

    private Mesh underWaterMesh;
    private Mesh aboveWaterMesh;
    
    private Rigidbody boatRb;

    private float waterDensity = BoatPhysicsMath.RHO_OCEAN_WATER;
    private float airDensity = BoatPhysicsMath.RHO_AIR;
    
    private void Awake()
    {
        boatRb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    private void Start()
    {

        // Initialization of script that will modify boat mesh
        modifyBoatMesh = new ModifyBoatMesh(boatMeshObject, underWaterObject, aboveWaterObject, boatRb);

        // Meshes that are below and above the water
        underWaterMesh = underWaterObject.GetComponent<MeshFilter>().mesh;
        aboveWaterMesh = aboveWaterObject.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    private void Update()
    {
        // generate underwater mesh
        modifyBoatMesh.GenerateUnderWaterMesh();

        // Display under water mesh
        modifyBoatMesh.DisplayMesh(underWaterMesh, "UnderWaterMesh", modifyBoatMesh.underWaterTriangleData);
        
        modifyBoatMesh.DisplayMesh(aboveWaterMesh, "AboveWater Mesh", modifyBoatMesh.aboveWaterTriangleData);
    }

    private void FixedUpdate()
    {

        boatRb.centerOfMass = centerOfMass;
        
        // add forces to underwater boat parts
        if (modifyBoatMesh.underWaterTriangleData.Count > 0)
        {
            AddUnderWaterForces();
        }

        if (modifyBoatMesh.aboveWaterTriangleData.Count > 0)
        {
            AddAboveWaterForces();
        }
    }

    // adds forces to surfaces underwater
    private void AddUnderWaterForces()
    {

        float Cf = BoatPhysicsMath.ResistanceCoefficient(waterDensity, boatRb.velocity.magnitude,
            modifyBoatMesh.CalculateUnderWaterLength());

        List<SlammingForceData> slammingForceData = modifyBoatMesh.slammingForceData;

        CalculateSlammingVelocities(slammingForceData);

        float boatArea = modifyBoatMesh.boatArea;
        float boatMass = 1f; // TODO: Replace with boat's total mass

        List<int> indexOfOriginalTriangle = modifyBoatMesh.indexOfOriginalTriangle;

        List<TriangleData> underWaterTriangles = modifyBoatMesh.underWaterTriangleData;

        // iterate over all underwater triangles and add buoyancy force
        for (int i = 0; i < underWaterTriangles.Count; i++)
        {
            TriangleData triangle = underWaterTriangles[i];

            // Vector3 buoyancyForce = CalculateBuoyancyForce(waterDensity, triangle);
            
            Vector3 forceToAdd = Vector3.zero;
            
            // hydrostatic force 
            forceToAdd += BoatPhysicsMath.BuoyancyForce(waterDensity, triangle);
            
            // viscous water resistance
            forceToAdd += BoatPhysicsMath.ViscousWaterResistanceForce(waterDensity, triangle, Cf);
            
            // pressure drag
            forceToAdd += BoatPhysicsMath.PressureDragForce(triangle);
            
            // slamming force
            int originalTriangleIndex = indexOfOriginalTriangle[i];

            SlammingForceData slammingData = slammingForceData[originalTriangleIndex];

            forceToAdd += BoatPhysicsMath.SlammingForce(slammingData, triangle, boatArea, boatMass);

            boatRb.AddForceAtPosition(forceToAdd, triangle.triangleCenter);


            // for debugging

            // display triangle normal vector
            // Debug.DrawRay(triangle.triangleCenter, triangle.normal * 3f, Color.white);

            // display force vector
            // Debug.DrawRay(triangle.triangleCenter, buoyancyForce.normalized * -3f, Color.blue);
        }
    }

    void AddAboveWaterForces()
    {
        List<TriangleData> aboveWaterTriangleData = modifyBoatMesh.aboveWaterTriangleData;

        for (int i = 0; i < aboveWaterTriangleData.Count; i++)
        {
            TriangleData triangle = aboveWaterTriangleData[i];
            
            Vector3 forceToAdd = Vector3.zero;
            
            // air resistance
            forceToAdd += BoatPhysicsMath.AirResistanceForce(airDensity, triangle, BoatPhysicsMath.C_d_flat_plate_perpendicular_to_flow);
            
            boatRb.AddForceAtPosition(forceToAdd, triangle.triangleCenter);
        }
    }

    void CalculateSlammingVelocities(List<SlammingForceData> slammingForceData)
    {
        for (int i = 0; i < slammingForceData.Count; i++)
        {
            slammingForceData[i].previousVelocity = slammingForceData[i].previousVelocity;

            Vector3 center = transform.TransformPoint(slammingForceData[i].triangleCenter);

            slammingForceData[i].velocity = BoatPhysicsMath.GetTriangleVelocity(boatRb, center);
        }
    }
}
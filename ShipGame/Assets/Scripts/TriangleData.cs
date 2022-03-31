using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public struct TriangleData
{
    // global transform of the corners of the triangle 
    public Vector3 vertexA;
    public Vector3 vertexB;
    public Vector3 vertexC;

    public Vector3 triangleCenter;

    public float distFromTriangleCenterToSurface;

    public Vector3 triangleNormalVect;

    public float triangleArea;

    // defines structure of TriangleData and calculates properties of the triangle
    public TriangleData(Vector3 vertA, Vector3 vertB, Vector3 vertC)
    {
        vertexA = vertA;
        vertexB = vertB;
        vertexC = vertC;

        triangleCenter = (vertA + vertB + vertC) / 3f;

        distFromTriangleCenterToSurface =
            Mathf.Abs(WaterController.instance.DistanceToWater(triangleCenter, Time.time));

        triangleNormalVect = Vector3.Cross(vertB - vertA, vertC + vertA);

        float lengthAC = Vector3.Distance(vertA, vertC);
        float lengthBC = Vector3.Distance(vertB, vertC);
        
        triangleArea = lengthAC * lengthBC * Mathf.Sin(Vector3.Angle(vertC - vertA, vertC - vertB) * Mathf.Deg2Rad) /
                       2f;
    }
}
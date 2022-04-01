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
        this.vertexA = vertA;
        this.vertexB = vertB;
        this.vertexC = vertC;

        this.triangleCenter = (vertA + vertB + vertC) / 3f;

        this.distFromTriangleCenterToSurface =
            Mathf.Abs(WaterController.instance.DistanceToWater(triangleCenter, Time.time));

        this.triangleNormalVect = Vector3.Cross(vertB - vertA, vertC - vertA).normalized;

        float lengthAB = Vector3.Distance(vertA, vertB);
        float lengthCA = Vector3.Distance(vertC, vertA);
        
        this.triangleArea = lengthAB * lengthCA * Mathf.Sin(Vector3.Angle(vertB - vertA, vertC - vertA) * Mathf.Deg2Rad) /
                       2f;
    }
}
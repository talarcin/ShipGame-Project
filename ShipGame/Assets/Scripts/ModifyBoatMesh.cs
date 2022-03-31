using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyBoatMesh : MonoBehaviour
{
    private Transform boatTransform;
    private Vector3[] boatVertices;
    private int[] boatTriangles;

    public Vector3[] boatVerticesGlobalPos;
    private float[] allDistancesToWater;
    
    public List<TriangleData> underWaterTriangleData = new List<TriangleData>();

    public ModifyBoatMesh(GameObject boatObject)
    {
        boatTransform = boatObject.transform;

        boatVertices = boatObject.GetComponent<MeshFilter>().mesh.vertices;
        boatTriangles = boatObject.GetComponent<MeshFilter>().mesh.triangles;

        boatVerticesGlobalPos = new Vector3[boatVertices.Length];
        allDistancesToWater = new float[boatVertices.Length];
    }
    
    public void GenerateUnderWaterMesh()
    {
        underWaterTriangleData.Clear();

        for (int i = 0; 0 < boatVertices.Length; i++)
        {
            Vector3 globalPos = boatTransform.TransformPoint(boatVertices[i]);

            boatVerticesGlobalPos[i] = globalPos;

            allDistancesToWater[i] = WaterController.instance.DistanceToWater(globalPos, Time.time);
        }

        AddTriangles();
    }

    private void AddTriangles()
    {
        throw new System.NotImplementedException();
    }


    public void DisplayMesh(Mesh mesh, string name, List<TriangleData> trianglesData)
    {
        
    }
}

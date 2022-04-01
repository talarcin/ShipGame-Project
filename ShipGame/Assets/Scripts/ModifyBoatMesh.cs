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

    // to store necessary information about triangle vertices
    public class VertexData
    {
        public int index;
        public float distanceToWater;
        public Vector3 globalPos;
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
        // list stores temporary data about current triangles vertices in following loop
        List<VertexData> vertexData = new List<VertexData>();

        // initialization, will be overwritten
        vertexData.Add(new VertexData());
        vertexData.Add(new VertexData());
        vertexData.Add(new VertexData());

        int i = 0;

        while (i < boatTriangles.Length)
        {
            // Loop over all three vertices of current triangle and store vertex data
            for (int j = 0; j < 3; j++)
            {
                vertexData[j].index = j;
                vertexData[j].distanceToWater = allDistancesToWater[boatTriangles[i]];
                vertexData[j].globalPos = boatVerticesGlobalPos[boatTriangles[i]];

                // since boatTriangles stores index of first vertex, it has to be increased by 3 for next
                i++;
            }

            // if all vertices are above the water, loop is started again
            if (vertexData[0].distanceToWater > 0 && vertexData[1].distanceToWater > 0 &&
                vertexData[2].distanceToWater > 0)
            {
                continue;
            }

            // check if all vertices are underwater and then save the triangle
            if (vertexData[0].distanceToWater < 0 && vertexData[1].distanceToWater < 0 &&
                vertexData[2].distanceToWater < 0)
            {
                // save the global positions of vertices temporarily
                Vector3 vertA = vertexData[0].globalPos;
                Vector3 vertB = vertexData[1].globalPos;
                Vector3 vertC = vertexData[2].globalPos;
                
                underWaterTriangleData.Add(new TriangleData(vertA, vertB, vertC));
            }
            // if less than three vertices are underwater
            else
            {
                // sorting list to lessen comparisons and make calculations easier (especially clipping)
                vertexData.Sort((vertX, vertY) => vertX.distanceToWater.CompareTo(vertY.distanceToWater));
                vertexData.Reverse();
                
                // check if only one vertex is above the water (need to only check second in list, since sorted descending)
                if (vertexData[1].distanceToWater > 0)
                {
                    AddTrianglesTwoAboveWater(vertexData);
                }
                // no need to check, since at this point it is obvious, that only one vertex is above the water
                else
                {
                    AddTrianglesOneAboveWater(vertexData);
                }
                
            }
            
            
        }
    }

    // building two new triangles from the old one (with one vertex above the water)
    private void AddTrianglesOneAboveWater(List<VertexData> vertexData)
    {
        
    }

    // building one new triangle from the old one (with two vertices above the water)
    private void AddTrianglesTwoAboveWater(List<VertexData> vertexData)
    {
        throw new System.NotImplementedException();
    }


    public void DisplayMesh(Mesh mesh, string name, List<TriangleData> trianglesData)
    {
    }
}
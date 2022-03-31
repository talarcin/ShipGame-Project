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

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateUnderWaterMesh()
    {
        
    }

    public void DisplayMesh(Mesh underWaterMesh, string underwaterMesh, object underWaterTriangleData)
    {
        
    }
}

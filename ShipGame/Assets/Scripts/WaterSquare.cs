using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSquare
{
    public Transform squareTransform;

    public MeshFilter terrainMeshFilter;

    private float size;
    private float spacing;
    private int width;

    public Vector3 centerPos;
    public Vector3[] vertices;

    public WaterSquare(GameObject waterSquareObj, float size, float spacing)
    {
        this.size = size;
        this.spacing = spacing;
        this.squareTransform = waterSquareObj.transform;
        this.terrainMeshFilter = squareTransform.GetComponent<MeshFilter>();

        // calculate properties of square
        width = (int) (size / spacing);
        width += 1;

        // center the sea
        float offset = -((width - 1) * spacing) / 2;
        Vector3 newPos = new Vector3(offset, squareTransform.position.y, offset);
        squareTransform.position += newPos;

        // save center position of the water square
        this.centerPos = waterSquareObj.transform.localPosition;

        // Now we generate the sea

        // for debugging and calculating time it takes to generate the meshes
        float startTime = System.Environment.TickCount;

        GenerateMesh();

        float timeToGenerateSea = (System.Environment.TickCount - startTime) / 1000f;

        Debug.Log("Sea was generated in " + timeToGenerateSea + " seconds.");

        // vertices are saved to update them in a thread
        this.vertices = terrainMeshFilter.mesh.vertices;
    }

    private void GenerateMesh()
    {
        List<Vector3[]> vertices = new List<Vector3[]>();
        List<int> triangles = new List<int>();

        for (int z = 0; z < width; z++)
        {
            vertices.Add(new Vector3[width]);

            for (int x = 0; x < width; x++)
            {
                Vector3 currentPoint = new Vector3();

                // get vertex coordinates
                currentPoint.x = x * spacing;
                currentPoint.z = z * spacing;
                currentPoint.y = squareTransform.position.y;

                vertices[z][x] = currentPoint;

                // no generation of a triangle, since the first coordinate on each row is not a full triangle,
                // only a point
                if (x <= 0 || z <= 0)
                {
                    continue;
                }

                // since each square consists of one triangle, we have to generate two
                triangles.Add(x + z * width);
                triangles.Add(x + (z - 1) * width);
                triangles.Add((x - 1) + (z - 1) * width);

                triangles.Add(x + z * width);
                triangles.Add((x - 1) + (z - 1) * width);
                triangles.Add((x - 1) + z * width);
            }
        }

        // unfold 2D-Array of vertices into a 1D-Array
        Vector3[] unfoldedVertices = new Vector3[width * width];

        int i = 0;

        // copy all elements of current 2D-Array to 1D-Array (unfoldedVertices)
        foreach (Vector3[] verts in vertices)
        {
            verts.CopyTo(unfoldedVertices, i * width);

            i++;
        }

        // Generate the new mesh object
        Mesh newMesh = new Mesh();
        newMesh.vertices = unfoldedVertices;
        newMesh.triangles = triangles.ToArray();

        // Ensure bounding volumes are correct and update normals to reflect the change
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();

        // Add generated mesh to this Gameobject
        terrainMeshFilter.mesh.Clear();
        terrainMeshFilter.mesh = newMesh;
        terrainMeshFilter.mesh.name = "Water Mesh";

        Debug.Log(terrainMeshFilter.mesh.vertices.Length);
    }

    // for updating the square from outside of a thread
    public void MoveSea(Vector3 oceanPos, float timeSinceStart)
    {
        Vector3[] vertices = terrainMeshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            // transform position from local to global
            Vector3 vertexGlobal = vertex + centerPos + oceanPos;

            vertex.y = WaterController.instance.GetWaveYPos(vertexGlobal, timeSinceStart);

            vertices[i] = vertex;
        }

        terrainMeshFilter.mesh.vertices = vertices;
        terrainMeshFilter.mesh.RecalculateNormals();
    }
}
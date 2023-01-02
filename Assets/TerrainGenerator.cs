using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("References")]
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;

    [Header("Terrain")]
    public Vector2 regionSize = new Vector2(100, 100);

    [Header("Grass")]
    public GameObject grass;
    public float radius = 0.5f;
    public int rejectionSamples = 6;

    Mesh mesh;

    private void Start()
    {
        CreateMesh();
        SpawnGrass();
    }

    public void CreateMesh()
    {
        mesh = new Mesh();
        mesh.Clear();

        mesh.vertices = PerlinNoise.GenerateVertices(regionSize);
        mesh.triangles = PerlinNoise.GenerateTriangles(regionSize);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void SpawnGrass()
    {
        foreach (Vector2 point in PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples))
        {
            if (Physics.Raycast(new Vector3(point.x, 100, point.y), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Instantiate(grass, hit.point, Quaternion.identity);
            }
        }
    }
}

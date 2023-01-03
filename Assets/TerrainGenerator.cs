using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("References")]
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;
    public NoiseDrawing noiseDrawing;

    public enum DrawingMode {Noise, Falloff};
    [Header("Mode")]
	public DrawingMode drawingMode;

    [Header("Terrain")]
    public Vector2 regionSize = new (100, 100);
    public bool terrain;
    public bool grass;
    public bool trees;

    [Header("Perlin Noise")]
    public float heightMultiplier;
    public float zoom;
    public int octaves;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;
    public int seed;
    public AnimationCurve heightCurve;
    public Vector2 offset;

    [Header("Falloff")]
    public float a;
    public float b;

    [Header("Grass")]
    public GameObject grassPrefab;
    public float radius;
    public int rejectionSamples = 6;

    Mesh mesh;

    private void Start()
    {
        mesh = new Mesh();

        meshFilter.mesh = mesh;
    }

    private void Update()
    {
        if (terrain)
            SpawnMesh();
    }

    private void OnValidate()
    {
        if (regionSize.x < 1)
            regionSize.x = 1;
        if (regionSize.y < 1)
            regionSize.y = 1;
        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 1)
            octaves = 1;
    }

    private void SpawnMesh()
    {
        mesh.Clear();

        Vector3[] vertices = PerlinNoise.GenerateVertices(regionSize, seed, zoom, octaves, persistance, lacunarity, heightMultiplier, heightCurve, offset);
        int[] triangles = GenerateTriangles(regionSize);

        if (drawingMode == DrawingMode.Noise)
            noiseDrawing.DrawNoise(regionSize, vertices);
        if (drawingMode == DrawingMode.Falloff)
            noiseDrawing.DrawNoise(regionSize, Falloff.GenerateFalloff(regionSize, a, b));

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        meshCollider.sharedMesh = mesh;
    }

    private void SpawnGrass()
    {
        foreach (Vector2 point in PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples))
        {
            if (Physics.Raycast(new Vector3(point.x, 100, point.y), Vector3.down, out RaycastHit hit, Mathf.Infinity))
            {
                Instantiate(grassPrefab, hit.point, Quaternion.identity);
            }
        }
    }

    private int[] GenerateTriangles(Vector2 regionSize)
    {
        int[] triangles = new int[(int)(regionSize.x * regionSize.y * 6)];

        int currentVertex = 0;
        int currentTriangle = 0;

        for (int z = 0; z < regionSize.y; z++, currentVertex++)
        {
            for (int x = 0; x < regionSize.x; x++, currentVertex++, currentTriangle += 6)
            {
                triangles[currentTriangle + 0] = currentVertex + 0;
                triangles[currentTriangle + 1] = currentVertex + (int)regionSize.x + 1;
                triangles[currentTriangle + 2] = currentVertex + 1;
                triangles[currentTriangle + 3] = currentVertex + 1;
                triangles[currentTriangle + 4] = currentVertex + (int)regionSize.x + 1;
                triangles[currentTriangle + 5] = currentVertex + (int)regionSize.x + 2;
            }
        }

        return triangles;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    public static Vector3[] GenerateVertices(Vector2 regionSize)
    {
        Vector3[] vertices;

        vertices = new Vector3[(int)((regionSize.x + 1) * (regionSize.y + 1))];

        for (int index = 0, z = 0; z <= regionSize.y; z++)
        {
            for (int x = 0; x <= regionSize.x; x++, index++)
            {
                float y = Mathf.PerlinNoise(x * 0.1f, z * 0.1f) * 1.1f;
                vertices[index] = new Vector3(x, y, z);
            }
        }

        return vertices;
    }

    public static int[] GenerateTriangles(Vector2 regionSize)
    {
        int[] triangles = new int[(int)(regionSize.x * regionSize.y * 6)];

        int currentVertex = 0;
        int currentTriangle = 0;

        for (int z = 0; z < regionSize.y; z++, currentVertex++)
        {
            for (int x = 0; x < regionSize.x; x++, currentVertex++)
            {
                triangles[currentTriangle + 0] = currentVertex + 0;
                triangles[currentTriangle + 1] = currentVertex + (int)regionSize.x + 1;
                triangles[currentTriangle + 2] = currentVertex + 1;
                triangles[currentTriangle + 3] = currentVertex + 1;
                triangles[currentTriangle + 4] = currentVertex + (int)regionSize.x + 1;
                triangles[currentTriangle + 5] = currentVertex + (int)regionSize.x + 2;

                currentTriangle += 6;
            }
        }

        return triangles;
    }
}

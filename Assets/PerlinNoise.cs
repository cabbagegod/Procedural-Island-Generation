// Sebastian Lague and Brackeys

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PerlinNoise
{
    public static Vector3[] GenerateVertices(Vector2 regionSize, int seed, float zoom, int octaves, float persistance, float lacunarity, float heightMultiplier, AnimationCurve heightCurve, Vector2 offset, Vector3[] falloff)
    {
        int width = (int)regionSize.x + 1;
        int length = (int)regionSize.y + 1;

        Vector3[] vertices = new Vector3[width * length];

        System.Random prng = new (seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for (int octaveOffset = 0; octaveOffset < octaves; octaveOffset++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[octaveOffset] = new Vector2(offsetX, offsetY);
        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = (regionSize.x) / 2f;
        float halfHeight = (regionSize.y) / 2f;

        for (int index = 0, z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++, index++)
            {
                float frequency = 1;
                float amplitude = 1;
                float y = 0;

                for (int octave = 0; octave < octaves; octave++)
                {
                    float sampleX = ((x - halfWidth) / zoom * frequency) + (octaveOffsets[octave].x * frequency);
                    float sampleY = ((z - halfHeight) / zoom * frequency) + (octaveOffsets[octave].y * frequency);
                    float perlinValue = (Mathf.PerlinNoise(sampleX, sampleY) * 2) - 1;
                    y += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (y > maxNoiseHeight)
                    maxNoiseHeight = y;
                else if (y < minNoiseHeight)
                    minNoiseHeight = y;

                vertices[index] = new Vector3(x, y, z);
            }
        }

        for (int index = 0, z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++, index++)
            {
                vertices[index].y = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, vertices[index].y);
                vertices[index].y = Mathf.Clamp01(vertices[index].y - falloff[index].y);
                vertices[index].y = heightCurve.Evaluate(vertices[index].y);
                vertices[index].y *= heightMultiplier;
            }
        }

        return vertices;
    }
}

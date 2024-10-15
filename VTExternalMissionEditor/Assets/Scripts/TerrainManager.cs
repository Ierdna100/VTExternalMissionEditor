using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;

    public const float vertexSpacing = 153.6f;
    public const float chunkSpacing = 3072f;

    public const int pixelsPerChunk = 1;

    public const float maxTerrainHeight = -80f;
    public const float minTerrainHeight = 6000f;

    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public Material material;

    public int number = 3;

    private void Awake()
    {
        instance = this;
        Call();
    }

    [ContextMenu("CALL")]
    private void Call()
    {
        GenerateFlatTerrain(number);
    }

    private void GenerateFlatTerrain(int chunkCount)
    {
        var rand = new System.Random();
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();

        int planeLength = chunkCount * pixelsPerChunk + 1;
        Vector3[] vertices = new Vector3[planeLength * planeLength];
        Vector2[] uv = new Vector2[planeLength * planeLength];
        for (int i = 0; i < planeLength; i++)
        {
            for (int j = 0; j < planeLength; j++)
            {
                vertices[i * planeLength + j] = new Vector3(i, (float)rand.NextDouble() % 1, j);
                uv[i * planeLength + j] = new Vector2((float)i /  planeLength, (float)j / planeLength);
            }
        }

        int length = planeLength - 1;
        int[] tris = new int[length * length * 2 * 3] ;
        int triangleIndex = 0;
        for (int i = 0; i < tris.Length / 6; i++)
        {
            int j = i / length;
            tris[triangleIndex++] = i + j;
            tris[triangleIndex++] = i + j + 1;
            tris[triangleIndex++] = i + j + planeLength;
            tris[triangleIndex++] = i + j + planeLength;
            tris[triangleIndex++] = i + j + 1;
            tris[triangleIndex++] = i + j + planeLength + 1;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = tris;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Ecosystem
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private int sizeOfMap;
        [SerializeField] private int height;
        [SerializeField] private int scale;

        private int _width;
        private int _depth;
        private Vector2 _perlinOffset;
        public bool _isDone;

        
        private void Start()
        {
            _width = sizeOfMap;
            _depth = sizeOfMap;
            _perlinOffset = new Vector2(Random.Range(0, 99999), Random.Range(0, 99999));
            Terrain terrain = GetComponent<Terrain>();
            terrain.terrainData = GenerateTerrain(terrain.terrainData);

            _isDone = true;
            //GameObject _water = GameObject.CreatePrimitive(PrimitiveType.Plane);
            //GenerateWater(terrain, _water);
        }

        private TerrainData GenerateTerrain(TerrainData terrainData)
        {
            terrainData.heightmapResolution = _width + 1;
            terrainData.size = new Vector3(_width, height, _depth);
            terrainData.SetHeights(0, 0, GenerateHeights());
            return terrainData;
        }

        private float[,] GenerateHeights()
        {
            float[,] heights = new float[_width, _depth];
            for(int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    heights[x, z] = CalculateHeights(x, z);
                }
            }
            return heights;
        }

        private float CalculateHeights(int x, int z)
        {
            float xCoord = (float) x / _width * scale + _perlinOffset.x;
            float zCoord = (float) z / _width * scale + _perlinOffset.y; 
            return Mathf.PerlinNoise(xCoord, zCoord);
        }
    }
}

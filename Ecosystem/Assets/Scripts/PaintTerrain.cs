using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ecosystem
{
    public class PaintTerrain : MonoBehaviour
    {
        [Serializable]
        public class SplatHeights
        {
            public int textureIndex;
            public int startingHeight;
        }

        [SerializeField] private TerrainGenerator terrainGenerator;

        public SplatHeights[] splatHeights;
        
        void Start()
        {
            var terrainData = GetComponent<Terrain>().terrainData;
            float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight,
                terrainData.alphamapLayers];

            for (int y = 0; y < terrainData.alphamapHeight; y++)
            {
                for (int x = 0; x < terrainData.alphamapWidth; x++)
                {
                    float terrainHeight = terrainData.GetHeight(y, x);
                    float[] splat = new float[splatHeights.Length];

                    for (int i = 0; i < splatHeights.Length; i++)
                    {
                        if (i == splatHeights.Length - 1)
                        {
                            splat[i] = 1;
                        } else if (terrainHeight >= splatHeights[i].startingHeight &&
                                   terrainHeight <= splatHeights[i + 1].startingHeight)
                        {
                            splat[i] = 1;
                        }
                    }

                    for (int j = 0; j < splatHeights.Length; j++)
                    {
                        splatmapData[x, y, j] = splat[j];
                    }
                }
            }
            
            terrainData.SetAlphamaps(0, 0, splatmapData);
        }
    }
}

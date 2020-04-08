using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [Header("Feature Generation")]
    public float FeatureNoise_X = 8f;
    public float FeatureNoise_Z = 8f;
    public float FeatureNoise_Amplitude = 0.5f;

    [Header("Texture Generation")]
    public float TextureNoise_X = 16f;
    public float TextureNoise_Z = 16f;
    public float TextureNoise_Amplitude = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            GenerateTerrain();
    }

    void GenerateTerrain()
    {
        GenerateTerrain_Internal_Heights();

        GenerateTerrain_Internal_Textures();
    }

    void GenerateTerrain_Internal_Heights()
    {
		// retrieve the terrain
		Terrain terrain = gameObject.GetComponent<Terrain>();

		// grab the height information
		float[,] terrainHeights = new float[terrain.terrainData.heightmapWidth,terrain.terrainData.heightmapHeight];

		int width = terrain.terrainData.heightmapWidth;
		int height = terrain.terrainData.heightmapHeight;

		// set some initial height data
		for (int x = 0; x < width; ++x)
		{
			for(int z = 0; z < height; ++z)
			{
				// apply noise
				terrainHeights[x, z] = FeatureNoise_Amplitude * Mathf.PerlinNoise(FeatureNoise_X * x / width, FeatureNoise_Z * z / height);
			}
		}

		// update the heights
		terrain.terrainData.SetHeights(0, 0, terrainHeights);        
    }

    void GenerateTerrain_Internal_Textures()
    {
		// retrieve the terrain
		Terrain terrain = gameObject.GetComponent<Terrain>();

        // create the array to hold the weights
        float[,,] splatmapWeights = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, terrain.terrainData.alphamapLayers];

        // retrieve the heights
        float[,] terrainHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        
		int width = terrain.terrainData.alphamapWidth;
		int height = terrain.terrainData.alphamapHeight;
        int numLayers = terrain.terrainData.alphamapLayers;

		for (int x = 0; x < width; ++x)
		{
			for(int z = 0; z < height; ++z)
			{
                // below 20% of max height - force to be sand
                if (terrainHeights[x, z] < 0.2f)
                {
                    splatmapWeights[x, z, 0] = 1f;
                    splatmapWeights[x, z, 1] = 0f;
                    splatmapWeights[x, z, 2] = 0f;
                    splatmapWeights[x, z, 3] = 0f;
                } // above 80% of max height - force to be snow
                else if (terrainHeights[x, z] > 0.8f)
                {
                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 0f;
                    splatmapWeights[x, z, 2] = 0f;
                    splatmapWeights[x, z, 3] = 1f;                    
                } // otherwise make it a mix of dirt and grass
                else
                {
                    float grassWeight = TextureNoise_Amplitude * Mathf.PerlinNoise(TextureNoise_X * x / width, TextureNoise_Z * z / height);

                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 1f - grassWeight;
                    splatmapWeights[x, z, 2] = grassWeight;
                    splatmapWeights[x, z, 3] = 0f;                    
                }
			}
		}

        // update the weights
        terrain.terrainData.SetAlphamaps(0, 0, splatmapWeights);
    }
}

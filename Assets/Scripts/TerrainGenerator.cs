using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public static TerrainGenerator instance;
    public Terrain terrain;

    [Header("Feature Generation")]
    public int NumPasses = 5;
    public float FeatureNoise_X = 8f;
    public float FeatureNoise_Z = 8f;
    public float FeatureNoise_Amplitude = 0.5f;

    [Header("Texture Generation")]
    public float TextureNoise_X = 16f;
    public float TextureNoise_Z = 16f;
    public float TextureNoise_Amplitude = 0.5f;

    [Header("Robot Spawns")]
    public GameObject robotPrefab;
    public List<GameObject> robotList = new List<GameObject>(7);


    [Header("Sphere List")]
    public GameObject prefabTree;
    public GameObject tree;

    [Header("Sphere List")]
    private int originalSphereList = 174;
    public List<GameObject> sphereList = new List<GameObject>();

    [Header("Tree List")]
    public List<GameObject> treePlot = new List<GameObject>();
    public List<GameObject> plantedTrees = new List<GameObject>();
    public List<GameObject> hungryTrees = new List<GameObject>();
    public List<GameObject> thirstyTrees = new List<GameObject>();

    [Header("Dome Info")]
    public int domeRadius = 75;
    public int safePlantDistance = 3;

    [Header("Shelters")]
    public GameObject prefabWaterShelter;
    public GameObject waterShelter;
    public GameObject prefabStorageShelter;
    public GameObject storageShelter;
    public GameObject prefabChargingStation;
    public GameObject chargingStation;


    Pathdata pd;

    // Start is called before the first frame update
    void Start()
    {
        pd = FindObjectOfType<Pathdata>();
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (sphereList.Count > 0)
            {
                foreach (GameObject gameObjects in sphereList)
                {
                    Destroy(gameObjects);
                }
                sphereList.Clear();
            }

            GenerateTerrain();
        }
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

        FindObjectOfType<Pathdata>().WorldSize = new Vector2Int(terrain.terrainData.heightmapWidth - 1, terrain.terrainData.heightmapHeight - 1);

		// grab the height information
		float[,] terrainHeights = new float[terrain.terrainData.heightmapWidth,terrain.terrainData.heightmapHeight];

		int width = terrain.terrainData.heightmapWidth;
		int height = terrain.terrainData.heightmapHeight;

        //terrain.terrainData.heightmapScale

		// set some initial height data
        for (int pass = 0; pass < NumPasses; ++pass)
        {
            for (int x = 0; x < width; ++x)
            {
                for(int z = 0; z < height; ++z)
                {
                    // apply noise
                    if (pass == 0)
                        terrainHeights[x, z] = FeatureNoise_Amplitude * Mathf.PerlinNoise(FeatureNoise_X * x / width, FeatureNoise_Z * z / height);
                  //else if (pass == 1)
                  //       {
                  // terrainHeights[x, z] += Random.Range(-0.01f, 0.01f);                        
                  //       }
                    else
                    {
                        terrainHeights[x, z] += 0.01f * FeatureNoise_Amplitude * Mathf.PerlinNoise(FeatureNoise_X * x / width, FeatureNoise_Z * z / height);
                    }
                }
            }
        }

        // loop 10 times
        // pick random x and z (within the heightmap)

		// update the heights
		terrain.terrainData.SetHeights(0, 0, terrainHeights);        
    }

    void GenerateTerrain_Internal_Textures()
    {
		// retrieve the terrain
	    terrain = gameObject.GetComponent<Terrain>();

        // create the array to hold the weights
        float[,,] splatmapWeights = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, terrain.terrainData.alphamapLayers];

        // retrieve the heights
        float[,] terrainHeights = terrain.terrainData.GetHeights(0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);
        
		int width = terrain.terrainData.alphamapWidth;
		int height = terrain.terrainData.alphamapHeight;
        int numLayers = terrain.terrainData.alphamapLayers;

        Vector2Int waterShelterLocation = new Vector2Int(35, 35);
        int waterShelterRadius = 8;
        int waterShelterRadiusSq = waterShelterRadius * waterShelterRadius;

        waterShelter = Instantiate(prefabWaterShelter);
        waterShelter.transform.position = new Vector3Int(35, 3, 35);

        Vector2Int storageShelterLocation = new Vector2Int(75, 35);
        int storageShelterRadius = 10;
        int storageShelterRadiusSq = storageShelterRadius * storageShelterRadius;

        storageShelter = Instantiate(prefabStorageShelter);
        storageShelter.transform.position = new Vector3(35, 2.6f, 74);

        Vector2Int chargingStationLocation = new Vector2Int(55, 55);
        int chargingStationRadius = 5;
        int chargingStationRadiusSq = chargingStationRadius * chargingStationRadius;

        chargingStation = Instantiate(prefabChargingStation);
        chargingStation.transform.position = new Vector3(55, 2.6f, 55);

        Pathdata pathdata = FindObjectOfType<Pathdata>();

        for (int x = 0; x < width; ++x)
		{
			for(int z = 0; z < height; ++z)
			{
                Vector3 nodePosition = transform.position;
                nodePosition = new Vector3(z * terrain.terrainData.heightmapScale.z, 
                                            terrainHeights[x, z] * terrain.terrainData.heightmapScale.y,
                                            x * terrain.terrainData.heightmapScale.x);   

                PathdataNode node = pathdata.CreateNode(x, z, nodePosition);

                int distFromCenterSq = (x - (width / 2)) * (x - (width / 2)) + (z - (height / 2)) * (z - (height / 2));

                float grassWeight = TextureNoise_Amplitude * Mathf.PerlinNoise(TextureNoise_X * x / width, TextureNoise_Z * z / height);

                if (distFromCenterSq > (domeRadius - safePlantDistance) * (domeRadius - safePlantDistance))
                {
                    node.blocking = true;
                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 1f - grassWeight;
                    splatmapWeights[x, z, 2] = 0.2f + grassWeight;
                    splatmapWeights[x, z, 3] = 0f;

                    continue;
                }

                int distToWaterShelterSq = (x - waterShelterLocation.x) * (x - waterShelterLocation.x) + (z - waterShelterLocation.y) * (z - waterShelterLocation.y);
                if (distToWaterShelterSq < waterShelterRadiusSq)
                {
                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 1f;
                    splatmapWeights[x, z, 2] = 0f;
                    splatmapWeights[x, z, 3] = 0f;

                    continue;
                }

                int distToStorageShelterSq = (x - storageShelterLocation.x) * (x - storageShelterLocation.x) + (z - storageShelterLocation.y) * (z - storageShelterLocation.y);
                if (distToStorageShelterSq < storageShelterRadiusSq)
                {
                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 1f;
                    splatmapWeights[x, z, 2] = 0f;
                    splatmapWeights[x, z, 3] = 0f;

                    continue;
                }

                int distToChargingStationSq = (x - chargingStationLocation.x) * (x - chargingStationLocation.x) + (z - chargingStationLocation.y) * (z - chargingStationLocation.y);
                if (distToChargingStationSq < (chargingStationRadiusSq))
                {
                    // if (distToBlockoutSq > treeRadiusSq)
                    // {
                    //     // Do tree spawning with higher scale factor on the noise
                    // }

                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 0.9f;
                    splatmapWeights[x, z, 2] = 0.1f;
                    splatmapWeights[x, z, 3] = 0f;

                    continue;
                }

                // below 20% of max height - force to be sand
                if (terrainHeights[x, z] < 0.014f)
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
                else if(node.blocking == false)
                {
                    // would normally use different scale factors other than TextureNoise_
                    float treeThreshold = 0.025f * Mathf.PerlinNoise(TextureNoise_X * x / width, TextureNoise_Z * z / height); // The block of this and below could potentially be used to spawn 
                    if (Random.Range(0f, 1f) < treeThreshold)                                                                  // objects/structures within the world.
                    {
                        // Spawn an object
                        tree = Instantiate(prefabTree);
                        tree.transform.position = nodePosition + (Vector3.up * 0.5f);
                        sphereList.Add(tree);
                        treePlot.Add(tree);
                    }
                    if(robotList.Count <= 7)
                    {
                        GameObject tempRobot = Instantiate(robotPrefab);
                        tempRobot.transform.position = nodePosition + (Vector3.up * 0.5f);
                        robotList.Add(tempRobot);
                    }

                    splatmapWeights[x, z, 0] = 0f;
                    splatmapWeights[x, z, 1] = 1f - grassWeight;
                    splatmapWeights[x, z, 2] = 0.2f + grassWeight;
                    splatmapWeights[x, z, 3] = 0f;                    
                }
			}
		}
        for(int x = 0; x < width; ++x)
        {
            for(int z = 0; z < height; ++z)
            {
                int pathDataSurroundingIndex = x + z * height; // just made it multiply by height as my terrain is a square meaning it is equal.

                if(pd.AllNodes[pathDataSurroundingIndex].blocking)
                {
                    continue;
                }

                var edge = GetPathDataNodeWithinFace(x + 1, z, height); // right one
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x + 1, z + 1, height); // top right
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x - 1, z, height); // left one
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x - 1, z + 1, height); //  top left
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x - 1, z - 1, height); // bottom left
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x + 1, z - 1, height); // bottom right
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x, z - 1, height); // bottom middle
                AddToEdges(edge, pathDataSurroundingIndex);

                edge = GetPathDataNodeWithinFace(x, z + 1, height); // top middle
                AddToEdges(edge, pathDataSurroundingIndex);
            }
        }

        // update the weights
        terrain.terrainData.SetAlphamaps(0, 0, splatmapWeights);
    }

    private void AddToEdges(PathdataNode edge, int pathDataSurroundingIndex)
    {
        if (edge != null && !edge.blocking)
        {
            pd.AllNodes[pathDataSurroundingIndex].listOfEdges.Add(new PathdataEdge(edge));
        }
        else
        {
            return;
        }
    }

    public PathdataNode GetPathDataNodeWithinFace(int faceX, int faceY, int mapSize)
    {
        if(faceX < 0 || faceY < 0 || faceX >= mapSize || faceY >= mapSize)
        {
            return null;
        }

        int pathDataSurroundingIndex = faceX + faceY * mapSize;

        return pd.AllNodes[pathDataSurroundingIndex];
    }


}

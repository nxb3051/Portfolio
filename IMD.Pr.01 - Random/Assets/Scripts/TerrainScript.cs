using UnityEngine;
using System.Collections;

public class TerrainScript : MonoBehaviour
{
    //stores the data for terrain
    private TerrainData terrainData;
    
    //stores data for terrain creation
    public Vector3 worldSize;
    private int resolution = 129;
    public Texture2D terrainTexture;

    //stores the height map for the terrain
    private float[,] heights;

    //variables for terrain animation
    public float startTime = 0.0f;
    public float timeStep = 0.01f;

	// Use this for initialization
	void Start ()
    {
        //terrain creation and texturing
        TerrainCollider data = gameObject.GetComponent<TerrainCollider>();
        if(data != null)
        {
            terrainData = data.terrainData;
            terrainData.size = worldSize;
            terrainData.heightmapResolution = resolution;
        }
        SplatPrototype[] textures = new SplatPrototype[1];
        textures[0] = new SplatPrototype();
        textures[0].texture = terrainTexture;
        terrainData.splatPrototypes = textures;

        heights = terrainData.GetHeights(0, 0, resolution, resolution);
    }

    //manipulates the height map via perlin noise
    void GenerateTerrain()
    {
        float xVal = 1f + startTime;
        float zVal = 1f + startTime;

        for(int x = 0; x < resolution; x++)
        {
            zVal += timeStep;
            xVal = 1f;
            for(int z = 0; z < resolution; z++)
            {
                xVal += timeStep;
                heights[x, z] = Mathf.PerlinNoise(xVal, zVal);
                //heights[x, z] = Mathf.PerlinNoise(xVal * 0.009f, zVal * 0.009f);
            }
        }

        terrainData.SetHeights(0, 0, heights);
    }
	
	
	// Update is called once per frame
	void Update ()
    {
        //shifts the terrain, and offsets the texture
        GenerateTerrain();
        startTime += 0.01f;
        SplatPrototype[] tempSplat = new SplatPrototype[1];
        SplatPrototype oldProto = terrainData.splatPrototypes[0];
        SplatPrototype newProto = new SplatPrototype();
        tempSplat[0] = newProto;
        newProto.texture = oldProto.texture;
        newProto.tileSize = oldProto.tileSize;
        newProto.normalMap = oldProto.normalMap;
        newProto.tileOffset = oldProto.tileOffset + new Vector2(0,1f);
        this.terrainData.splatPrototypes = tempSplat;
    }

    //returns height of a given point on the terrain
    public float GetHeight(Vector3 position)
    {
        return Terrain.activeTerrain.SampleHeight(position);
    }
}

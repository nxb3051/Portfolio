using UnityEngine;
using System.Collections;

public class TerrainScript : MonoBehaviour {

    private TerrainData myTerrainData;
    public Vector3 worldSize;

    public Texture2D texture;

	// Use this for initialization
	void Start () {
        TerrainCollider terrainCollider = gameObject.GetComponent<TerrainCollider>();

        myTerrainData = terrainCollider.terrainData;
        myTerrainData.size = worldSize;

        SplatPrototype[] terrainTexture = new SplatPrototype[1];
        terrainTexture[0] = new SplatPrototype();
        terrainTexture[0].texture = texture;
        myTerrainData.splatPrototypes = terrainTexture;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public float GetHeight(Vector3 position)
    {
        return Terrain.activeTerrain.SampleHeight(position);
    }
}

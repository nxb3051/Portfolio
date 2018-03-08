using UnityEngine;
using System.Collections;

public class ObjectGeneration : MonoBehaviour {

    //Models used for storing the desired gameobject and cloning it repeatedly
    public GameObject model;
    private GameObject clone;

    //stores all teh cloned objects
    private GameObject[] trees;

    //stores the terrain
    public Terrain terrain;

    // Use this for initialization
    void Start()
    {
        //initializes each clone object, and set its location randomly before storing it in an array
        trees = new GameObject[35];
        for (int i = 0; i < 35; i++)
        {
            clone = Instantiate(model, this.transform.position, Quaternion.identity) as GameObject;
            clone.transform.parent = gameObject.transform;
            clone.transform.position += new Vector3(-clone.transform.position.x, 0, -clone.transform.position.z);
            clone.transform.position += new Vector3(Random.Range(0, terrain.terrainData.size.x), 0, Random.Range(0, terrain.terrainData.size.z));
            trees[i] = clone;
        }
    }

    //Provided by professor, produces a float that is the standard deviation from the mean.
    float Gaussian(float mean, float stdDev)
    {
        float val1 = Random.Range(0f, 1f);
        float val2 = Random.Range(0f, 1f);
        float gaussValue = Mathf.Sqrt(-2.0f * Mathf.Log(val1)) * Mathf.Sin(2.0f * Mathf.PI * val2);
        return mean + stdDev * gaussValue;
    }

    // Update is called once per frame
    void Update()
    {
        //iterates through the stored objects and moves them along with the terrain to simulate motion, wrapping them to the terrain's worldsize
        for (int i = 0; i < trees.Length; i++)
        {
            if(trees[i].transform.position.z < 0f)
            {
                trees[i].transform.position += new Vector3(0, 0, 200f);
            }
            else
            {
                trees[i].transform.position += new Vector3(0, 0, -1f);
            }
            trees[i].transform.position += new Vector3(0, (terrain.GetComponent<TerrainScript>().GetHeight(trees[i].transform.position) - trees[i].transform.position.y) + trees[i].transform.localScale.y, 0);
        }
    }
}

using UnityEngine;
using System.Collections;

public class LeaderGeneration : MonoBehaviour {

    //models used for storing the desired gameobject and cloning it repeatedly
    public GameObject model1;
    private GameObject clone;

    //array for storing the objects
    private GameObject[] leaders;

    //stores the terrain
    public Terrain terrain;

	// Use this for initialization
	void Start ()
    {
        //creates a specific number of clones, places them randomly according to the specified rules and stores them in the leader array
        leaders = new GameObject[10];
        for (int i = 0; i < 10; i++)
        {
            clone = Instantiate(model1, this.transform.position, Quaternion.identity) as GameObject;
            clone.transform.parent = gameObject.transform;
            clone.transform.localScale += new Vector3(0, Gaussian(0f, 0.25f), 0);
            clone.transform.position += new Vector3(Gaussian(0f, 1.5f), 0, -i * 7);
            leaders[i] = clone;
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
        //iterates through all of the objects stored in the array
        for (int i = 0; i < leaders.Length; i++)
        {
            //sets the new height to match the terrain height
            leaders[i].transform.position += new Vector3(0, (terrain.GetComponent<TerrainScript>().GetHeight(leaders[i].transform.position) - leaders[i].transform.position.y) + leaders[i].transform.localScale.y + 0.3f, 0);
            //manipulates the rotation so that the object appears to hug the terrain
            RaycastHit rayHit;
            Vector3 theRay = leaders[i].transform.TransformDirection(Vector3.down);
            if(Physics.Raycast(leaders[i].transform.position, theRay, out rayHit))
            {
                leaders[i].transform.rotation = new Quaternion(rayHit.normal.x,rayHit.normal.y,rayHit.normal.z,0);
                leaders[i].transform.Rotate(new Vector3(0, leaders[i].transform.rotation.y, 0), 180f);
            }
        }
    }
}

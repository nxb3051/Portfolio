using UnityEngine;
using System.Collections;

public class HordeGeneration : MonoBehaviour {

    //Models used for storing the desired gameobject and cloning it repeatedly
    public GameObject model1;
    private GameObject clone;

    //stores all of the cloned objects
    private GameObject[] horde;

    //stores the terrain
    public Terrain terrain;

	// Use this for initialization
	void Start ()
    {
        //creates a specific number of clones, places them randomly according to the specified rules and stores them in the horde array
        horde = new GameObject[60];
        for (int i = 0; i < 60; i++)
        {
            clone = Instantiate(model1, this.transform.position, Quaternion.identity) as GameObject;
            clone.transform.parent = gameObject.transform;
            clone.transform.position += new Vector3(Random.Range(-40f, 40f), 0, -Mathf.Abs(Gaussian(0f, 30f)));
            horde[i] = clone;
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
        for (int i = 0; i < horde.Length; i++)
        {
            //sets the new height to match the terrain height
            horde[i].transform.position += new Vector3(0, (terrain.GetComponent<TerrainScript>().GetHeight(horde[i].transform.position) - horde[i].transform.position.y) + horde[i].transform.localScale.y + 0.3f, 0);
            //manipulates the rotation so that the object appears to hug the terrain
            RaycastHit rayHit;
            Vector3 theRay = horde[i].transform.TransformDirection(Vector3.down);
            if (Physics.Raycast(horde[i].transform.position, theRay, out rayHit))
            {
                horde[i].transform.rotation = new Quaternion(rayHit.normal.x, rayHit.normal.y, rayHit.normal.z, 0);
                horde[i].transform.Rotate(new Vector3(0, horde[i].transform.rotation.y, 0), 180f);
            }
        }
    }
}

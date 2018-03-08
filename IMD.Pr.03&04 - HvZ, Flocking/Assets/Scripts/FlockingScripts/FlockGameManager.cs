using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockGameManager : MonoBehaviour {

    //stores all instances of the game objects
    public int flockCount;
    public int obstacleCount;
    private List<GameObject> flockers = new List<GameObject>();
    private List<GameObject> obstacles = new List<GameObject>();
    private GameObject target;
    private GameObject flockCenter;

    //stores the second camera
    public Camera cam;

    //stores all of the models used for instantiation
    public GameObject centerModel;
    public GameObject flockModel;
    public GameObject targetModel;
    public GameObject obstacleModel;

    //stores world data
    public Terrain terrain;
    private TerrainScript terrainScript;
    public Vector3 worldSize;

    //stores material for debug line
    public Material matPurple;

	// Use this for initialization
	void Start () {
        terrainScript = terrain.GetComponent<TerrainScript>();
        worldSize = terrainScript.worldSize;

        //creates all the trees
        for (int i = 0; i < obstacleCount; i++)
        {
            obstacles.Add(GameObject.Instantiate(obstacleModel));
            RandomizePosition(obstacles[i]);
        }

        //creates the target
        target = GameObject.Instantiate(targetModel);
        RandomizePosition(target);

        //initializes all of the objects in the flock
        for (int i = 0; i < flockCount; i++)
        {
            flockers.Add(GameObject.Instantiate(flockModel));
            RandomizePosition(flockers[i]);

            FlockMovement mScript = flockers[i].GetComponent<FlockMovement>();
            mScript.SetTarget(target);
            mScript.SetObstacleList(obstacles);
            mScript.SetPalList(flockers);
            mScript.mass = 1.0f;
            mScript.maxSpeed = 20.0f;
        }

        //initializes the flock center object
        flockCenter = GameObject.Instantiate(centerModel);
        flockCenter.GetComponent<FlockFollower>().setFlock(flockers);
        flockCenter.GetComponent<FlockFollower>().setCam(cam);
    }
	
	// Update is called once per frame
	void Update () {
        CheckCollisions();
	}

    void RandomizePosition(GameObject theObject)
    {
        Vector3 position = new Vector3(Random.Range(10.0f, worldSize.x-10.0f), Random.Range(10.0f, worldSize.y-10.0f), Random.Range(10.0f, worldSize.z-10.0f));
        theObject.transform.position = position;
    }

    //checks for collisions between each human and zombie
    void CheckCollisions()
    {
        for (int i = 0; i < flockers.Count; i++)
        {
            Vector3 dist = flockers[i].transform.position - target.transform.position;
            if (dist.magnitude < 1f)
            {
                RandomizePosition(target);
            }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagement : MonoBehaviour {

    //stores all of the objects as well as counts of each object
    public int humanCount;
    public int zombieCount;
    public int treeCount;
    public List<GameObject> humans;
    public List<GameObject> zombies;
    public List<GameObject> obstacles;

    //the original models for each
    public GameObject humanModel;
    public GameObject zombieModel;
    public GameObject treeModel;

    //toggle for the debug lines
    private bool debug = false;

    //terrain information
    public Terrain terrain;
    private TerrainScript terrainScript;
    public Vector3 worldSize;

    //materials used for debug lines
    public Material matRed;
    public Material matGreen;
    public Material matBlue;
    public Material matBlack;
    public Material matPurple;

    // Use this for initialization
    void Start () {
        terrainScript = terrain.GetComponent<TerrainScript>();
        worldSize = terrainScript.worldSize;

        //creates all the trees
        for(int i = 0; i < treeCount; i++)
        {
            obstacles.Add(GameObject.Instantiate(treeModel));
            RandomizePosition(obstacles[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //toggles the debug lines
        if (Input.GetKeyDown(KeyCode.D))
        {
            debug = !debug;
        }
        //initializes a zombie
        if (Input.GetKeyDown(KeyCode.Z))
        {
            zombies.Add(GameObject.Instantiate(zombieModel));
            zombieCount++;
            Movement mScript = zombies[zombieCount-1].GetComponent<Movement>();
            mScript.SetTargetList(humans);
            mScript.SetPalList(zombies);
            mScript.SetObstacleList(obstacles);
            RandomizePosition(zombies[zombieCount-1]);
            mScript.isZombie = true;
            mScript.mass = 1;
            mScript.maxSpeed = 50.0f;
        }
        //initializes a human
        if (Input.GetKeyDown(KeyCode.H))
        {
            humans.Add(GameObject.Instantiate(humanModel));
            humanCount++;
            Movement mScript = humans[humanCount-1].GetComponent<Movement>();
            mScript.SetTargetList(zombies);
            mScript.SetPalList(humans);
            mScript.SetObstacleList(obstacles);
            RandomizePosition(humans[humanCount-1]);
            mScript.isZombie = false;
            mScript.mass = 1;
            mScript.maxSpeed = 50.0f;
        }
        //allow to quit the scene
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //updates a zombie's target list as well as whether it should have it's debug lines
        foreach (GameObject z in zombies)
        {
            z.GetComponent<Movement>().SetTarget(UpdateTarget(z));
            z.GetComponent<Movement>().Lines = debug;
        }
        //updates a human's target list as well as whether it should have it's debug lines
        foreach(GameObject h in humans)
        {
            h.GetComponent<Movement>().SetTarget(UpdateTarget(h));
            h.GetComponent<Movement>().Lines = debug;
        }
        //checks collisions between each human to each zombie
        CheckCollisions();
	}

    //randomizes an objects position in the world
    void RandomizePosition(GameObject theObject)
    {
        Vector3 position = new Vector3(Random.Range(0.0f, worldSize.x), 0.0f, Random.Range(0.0f, worldSize.z));
        position.y = terrainScript.GetHeight(position) + 1.0f;
        theObject.transform.position = position;
    }

    //updates the closest target that either a zombie should pursue, or a human should evade
    GameObject UpdateTarget(GameObject seeker)
    {
        GameObject closest = null;
        if (seeker.GetComponent<Movement>().isZombie)
        {
            if (humanCount > 0)
            {
                closest = humans[0];
                for (int i = 1; i < humanCount; i++)
                {
                    if ((humans[i].transform.position - seeker.transform.position).magnitude < (closest.transform.position - seeker.transform.position).magnitude)
                    {
                        closest = humans[i];
                    }
                }
            }
        }
        else
        {
            foreach(GameObject z in zombies)
            {
                Vector3 dist = z.transform.position - seeker.transform.position;
                if (dist.magnitude < 30f)
                {
                    if(closest == null || dist.magnitude < (closest.transform.position - seeker.transform.position).magnitude)
                    {
                        closest = z;
                    }
                }
            }
        }
        return closest;
    }

    //checks for collisions between each human and zombie
    void CheckCollisions()
    {
        if (zombieCount > 0 && humanCount > 0)
        {
            for(int i = 0; i < humanCount; i++)
            {
                for(int k = 0; k < zombieCount; k++)
                {
                    Vector3 dist = zombies[k].transform.position - humans[i].transform.position;
                    if (dist.magnitude < 1f)
                    {
                        Conversion(humans[i]);
                        i--;
                    }
                }
            }
        }
    }

    //code to turn a human into a zombie. Side note, it just destroys the human and makes a new zombie in it's place.
    void Conversion(GameObject human)
    {
        Vector3 pos = human.transform.position;
        humans.Remove(human);
        humanCount--;
        zombieCount++;
        GameObject zomb = GameObject.Instantiate(zombieModel);
        zombies.Add(zomb);
        Movement mScript = zomb.GetComponent<Movement>();
        mScript.SetTargetList(humans);
        mScript.SetPalList(zombies);
        mScript.SetObstacleList(obstacles);
        zomb.transform.position = pos;
        mScript.isZombie = true;
        mScript.mass = 1;
        mScript.maxSpeed = 50.0f;
        GameObject.Destroy(human);
    }
}

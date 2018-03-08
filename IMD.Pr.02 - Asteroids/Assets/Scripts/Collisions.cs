using UnityEngine;
using System.Collections;

public class Collisions : MonoBehaviour {
    //handles collisions between stuff and what happens

    //stores all collidable objects
    public GameObject player;
    private GameObject[] asteroids;
    private GameObject[] bullets;

    //stores singletons for their methods
    private GameObject gameManager;
    private GameObject asteroidManager;

    //keeps track of distance between two objects
    private Vector3 objDist;

    //radii of the player and asteroids respectively
    private float pRadius;
    private float aRadius;

	// Use this for initialization
	void Start () {
        //sets initial variables, player radius is always constant
        pRadius = player.transform.localScale.y/5.5f;
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        asteroidManager = GameObject.FindGameObjectWithTag("AsteroidManager");
    }
	
	// Update is called once per frame
	void Update () {
        //gets all bullets and asteroids currently on screen
        asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        bullets = GameObject.FindGameObjectsWithTag("Bullet");
        //only checks for collisions if there are asteroids
        if (asteroids != null)
        {
            //cycles through all asteroids
            for(int i = 0; i < asteroids.Length; i++)
            {
                //gets distance from asteroid to player
                aRadius = asteroids[i].transform.localScale.y/2.5f;
                objDist = asteroids[i].transform.position - player.transform.position;
                if(objDist.magnitude < pRadius + aRadius)
                {
                    //handles player collision, destroys the asteroid and breaks from the for loop
                    player.GetComponent<PlayerMovement>().PlayerHit();
                    gameManager.GetComponent<Lives>().ChangeLife(-1);
                    Destroy(asteroids[i]);
                    break;
                }
                //only checks for collision with bullets if there are any
                if (bullets != null)
                {
                    foreach (GameObject bullet in bullets)
                    {
                        //checks if bullet is colliding with asteroid
                        objDist = bullet.transform.position - asteroids[i].transform.position;
                        if (objDist.magnitude < aRadius)
                        {
                            //destroys asteroid if it's smaller
                            if (asteroids[i].transform.localScale.x <= 1f)
                            {
                                gameManager.GetComponent<Score>().RaiseScore(50);
                                Destroy(asteroids[i]);
                            }
                            //splits the asteroid if it's bigger
                            else
                            {
                                gameManager.GetComponent<Score>().RaiseScore(20);
                                asteroidManager.GetComponent<Asteroid>().AsteroidSplit(asteroids[i]);
                                Destroy(bullet);
                            }
                        }
                    }
                }
            }
        }
	}
}

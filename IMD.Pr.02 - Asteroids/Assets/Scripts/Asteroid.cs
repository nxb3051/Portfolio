using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
    //handles asteroid creation, as well as splitting hit asteroids

    //asteroid spawn count
    public int numAst = 3;

    //asteroid creation data
    public Object original;
    public Sprite[] templates;
    public GameObject[] asteroids;

    public Camera cam;

	// Use this for initialization
	void Start () {
        //spawns initial asteroids
        SpawnAsteroids(numAst);
        numAst++;
	}
	
	// Update is called once per frame
	void Update () {
        //spawns new asteroids if there aren't any
        if(GameObject.FindGameObjectsWithTag("Asteroid").Length == 0)
        {
            SpawnAsteroids(numAst);
            numAst++;
        }
	}

    //makes "j" number of asteroids and sets their various data
    public void SpawnAsteroids(int j)
    {
        for (int i = 0; i < j; i++)
        {
            GameObject ast = (GameObject)Instantiate(original, Vector3.zero, Quaternion.identity);
            ast.GetComponent<SpriteRenderer>().sprite = templates[Random.Range(0, templates.Length)];
            ast.transform.parent = this.transform;
            ast.transform.Rotate(Vector3.forward, Random.Range(0f, 360f));
            ast.transform.position = cam.ViewportToWorldPoint(new Vector3(Random.Range(0.2f, 1f), Random.Range(0f, 1f), 10));
            ast.tag = "Asteroid";
        }
    }

    //spawns two new smaller asteroids in the same relative direction of a main asteroid, which is destroyed
    public void AsteroidSplit(GameObject asteroid)
    {
        GameObject ast = (GameObject)Instantiate(asteroid, asteroid.transform.position, asteroid.transform.rotation);
        ast.transform.Rotate(Vector3.forward, Random.Range(0, 50));
        ast.transform.localScale /= 2;
        ast.tag = "Asteroid";
        ast.transform.parent = this.transform;
        ast.GetComponent<SpriteRenderer>().sprite = templates[Random.Range(0, templates.Length)];
        ast = (GameObject)Instantiate(asteroid, asteroid.transform.position, asteroid.transform.rotation);
        ast.transform.Rotate(Vector3.forward, Random.Range(-50, 0));
        ast.transform.localScale /= 2;
        ast.tag = "Asteroid";
        ast.transform.parent = this.transform;
        ast.GetComponent<SpriteRenderer>().sprite = templates[Random.Range(0, templates.Length)];
        Destroy(asteroid);
    }
}

using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour {
    //handles asteroid movement

    //movement data for the asteroids
    public int speed = 3;
    private Vector3 velocity = new Vector3(1f, 0f, 0f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //moves the asteroid
        transform.position += transform.rotation * velocity * speed * Time.deltaTime;
	}
}

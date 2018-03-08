using UnityEngine;
using System.Collections;

public class Wrapping : MonoBehaviour {
    //wraps the object to the screen

    //gets bounds data from cam
    private Camera cam;
    Vector3 temp;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        //ensures that when object goes outside of the visible screen, its position is set to the opposite side
        temp = cam.WorldToViewportPoint(transform.position);
        if (temp.x > 1)
        {
            transform.position = new Vector3(-transform.position.x + 0.1f, transform.position.y, transform.position.z);
        }
        if(temp.x < 0)
        {
            transform.position = new Vector3(-transform.position.x - 0.1f, transform.position.y, transform.position.z);
        }
        if (temp.y > 1)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y + 0.1f, transform.position.z);
        }
        if(temp.y < 0)
        {
            transform.position = new Vector3(transform.position.x, -transform.position.y - 0.1f, transform.position.z);
        }
    }
}

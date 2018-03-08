using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    //creates a bullet

    public Object original;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        //when space is pressed, creates a bullet
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bul = (GameObject)Instantiate(original, transform.position, transform.rotation);
            bul.tag = "Bullet";
        }	
	}
}

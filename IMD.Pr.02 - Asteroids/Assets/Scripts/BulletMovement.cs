using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {
    //handles bullet movement

    //stores movement data
    public int speed = 30;
    private Vector3 velocity = new Vector3(0f, 1f, 0f);

    private float lifeSpan = 0.3f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //moves the bullet, destroys it after a couple of seconds
        transform.position += transform.rotation * velocity * speed * Time.deltaTime;
        lifeSpan -= 1f * Time.deltaTime;
        if(lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }
}

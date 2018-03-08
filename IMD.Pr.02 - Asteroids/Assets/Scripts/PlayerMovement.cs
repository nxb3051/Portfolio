using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
    //handles player movement

    //movement vectors
    Vector3 movement = Vector3.zero;
    Vector3 acceleration = new Vector3(0f, 0.2f, 0f);

    //ships direction
    Quaternion direction;

    float angle = 160;
    private float deceleration = 0.995f;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //rotates the ship left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.RotateAround(transform.position, Vector3.forward, angle * Time.deltaTime);
        }
        //rotates the ship right
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(transform.position, Vector3.forward, -angle * Time.deltaTime);
        }
        //handles forward movement
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            direction = transform.rotation;
            movement += direction * acceleration;
            movement = Vector3.ClampMagnitude(movement, 20);
        }
        //handles deceleraton
        else
        {
            movement *= deceleration;
        }
        transform.position += movement * Time.deltaTime;
    }

    //resets movement vectors and player position when hit
    public void PlayerHit()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        movement = Vector3.zero;
    }
}

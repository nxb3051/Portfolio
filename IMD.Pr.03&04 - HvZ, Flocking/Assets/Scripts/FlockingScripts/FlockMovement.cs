using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockMovement : MonoBehaviour {

    //movement vectors
    private Vector3 direction;
    private Vector3 acceleration;
    private Vector3 velocity;
    private Vector3 position;

    //force constants
    public float mass;
    public float maxSpeed;

    //makes sure the object is in the world
    public bool inWorld = true;

    //stores other objects necessary for proper functioning
    private FlockGameManager gameManager;
    private Vector3 worldSize;
    private GameObject target;
    private List<GameObject> palList;
    private List<GameObject> obstacles;

    // Use this for initialization
    void Start () {
        //initializes all of the good stuff
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FlockGameManager>();
        position = transform.position;
        worldSize = gameManager.worldSize;

        if (mass <= 0.0f)
        {
            mass = 0.01f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        CheckBounds();
        UpdatePosition();
        SetTransform();
    }

    //handles behavior
    void UpdatePosition()
    {
        position = transform.position;

        if (inWorld)
        {
            //if in world, seek the target
            Seek(target);
        }
        else
        {
            //if outside the world, return to center
            ReturnToCenter();
        }
        //keeps separate from same objects, but also together and aligned in the right direction and avoids obstacles
        Separate();
        Cohesion();
        Alignment();
        ObstacleAvoidance();

        //applies movement vectors to simulate movement
        velocity += acceleration * Time.deltaTime;
        if (velocity.magnitude > maxSpeed)
        {
            Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        direction = velocity.normalized;
    }

    //sets the list of same objects to keep separate from
    public void SetPalList(List<GameObject> list)
    {
        palList = list;
    }

    //sets the list of obstacles to avoid
    public void SetObstacleList(List<GameObject> list)
    {
        obstacles = list;
    }

    //sets the target of the object's steering behaviors
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    //applies a steering force to acceleration
    void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
        Vector3.ClampMagnitude(acceleration, maxSpeed);
    }

    //moves the transform component
    void SetTransform()
    {
        transform.position = position;
        transform.right = direction;
    }

    //creates and follows a steering force directed towards an object's future position
    void Seek(GameObject target)
    {
        Vector3 targetPosition = target.transform.position;
        Vector3 desiredVelocity = targetPosition - position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        ApplyForce(steeringForce);
    }

    //determines where an object within a certain radius is and applies a force away from it
    void ObstacleAvoidance()
    {
        Vector3 steeringForce = Vector3.zero;
        if (obstacles.Count > 0)
        {
            foreach (GameObject obs in obstacles)
            {
                Vector3 dist = obs.transform.position - position;
                if (dist.magnitude < 5f)
                {
                    if (Vector3.Dot(dist, transform.right) > 0)
                    {
                        if (Vector3.Dot(dist, transform.forward) > 0)
                        {
                            steeringForce += (transform.forward * -1f) * maxSpeed * 2;
                        }
                        else
                        {
                            steeringForce += transform.forward * maxSpeed * 2;
                        }
                    }
                }
            }
        }
        ApplyForce(steeringForce);
    }

    //applies forces away from any similar objects within a specified radius
    void Separate()
    {
        Vector3 steeringForce = Vector3.zero;
        if (palList.Count > 1)
        {
            List<GameObject> neighbors = new List<GameObject>();
            for (int i = 0; i < palList.Count; i++)
            {
                if (!palList[i].Equals(gameObject))
                {
                    if ((palList[i].transform.position - position).magnitude < 5.0f)
                    {
                        neighbors.Add(palList[i]);
                    }
                }
            }
            if (neighbors.Count > 0)
            {
                foreach (GameObject pal in neighbors)
                {
                    steeringForce += (position - pal.transform.position).normalized * 5f;
                }
            }
        }
        ApplyForce(steeringForce);
    }

    //applies a force that encourages objects to stick together with other objects within a certain distance
    void Cohesion()
    {
        Vector3 steeringForce = Vector3.zero;
        if(palList.Count > 1)
        {
            List<GameObject> neighbors = new List<GameObject>();
            for (int i = 0; i < palList.Count; i++)
            {
                if (!palList[i].Equals(gameObject))
                {
                    if ((palList[i].transform.position - position).magnitude < 20.0f)
                    {
                        neighbors.Add(palList[i]);
                    }
                }
            }
            if (neighbors.Count > 0)
            {
                foreach (GameObject pal in neighbors)
                {
                    steeringForce += pal.transform.position;
                }
                steeringForce /= neighbors.Count;
                steeringForce -= position;
            }
        }
        ApplyForce(steeringForce);
    }

    //applies a force to encourage objects to align themselves with others in a certain range
    void Alignment()
    {
        Vector3 steeringForce = Vector3.zero;
        if (palList.Count > 1)
        {
            List<GameObject> neighbors = new List<GameObject>();
            for (int i = 0; i < palList.Count; i++)
            {
                if (!palList[i].Equals(gameObject))
                {
                    if ((palList[i].transform.position - position).magnitude < 20.0f)
                    {
                        neighbors.Add(palList[i]);
                    }
                }
            }
            if (neighbors.Count > 0)
            {
                foreach (GameObject pal in neighbors)
                {
                    steeringForce += pal.GetComponent<FlockMovement>().velocity;
                }
                steeringForce /= neighbors.Count;
                steeringForce -= velocity;
            }
        }
        ApplyForce(steeringForce);
    }

    //checks if an object is within the world
    void CheckBounds()
    {
        if (position.x > worldSize.x - 10 || position.x < 10 || position.z > worldSize.z - 10 || position.z < 10 || position.y > worldSize.y - 10 || position.y < 10)
        {
            inWorld = false;
        }
        else
        {
            inWorld = true;
        }
    }

    //adds movement force to push objects back into the world
    void ReturnToCenter()
    {
        Vector3 center = new Vector3(worldSize.x / 2 - position.x, worldSize.y / 2 - position.y, worldSize.z / 2 - position.z);
        center.Normalize();
        center *= maxSpeed;
        ApplyForce(center);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Movement : MonoBehaviour {

    //movement vectors
    private Vector3 direction;
    private Vector3 acceleration;
    private Vector3 velocity;
    private Vector3 position;

    //debug line toggle
    private bool lines = false;

    //force constants
    public float mass;
    public float maxSpeed;

    //various bools to determine actions to take
    public bool inWorld = true;
    public bool isZombie;
    public bool seekOrFlee = false;

    //ambient information, like who to avoid, who to keep separate from, etc.
    private GameManagement gameManager;
    private Vector3 worldSize;
    private GameObject target;
    private List<GameObject> targetList;
    private List<GameObject> palList;
    private List<GameObject> obstacles;

    //properties for the debug lines and the velocity
    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public bool Lines
    {
        get { return lines; }
        set { lines = value; }
    }

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagement>();
        position = transform.position;
        worldSize = gameManager.worldSize;

        if (mass <= 0.0f)
        {
            mass = 0.01f;
        }
    }
	
	//checks to see if in the world, and then moves accordingly
	void Update () {
        CheckBounds();
        UpdatePosition();
        SetTransform();
	}

    //sets the target of the object's steering behaviors
    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    //sets the list of potential targets
    public void SetTargetList(List<GameObject> list)
    {
        targetList = list;
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

    //handles behavior
    void UpdatePosition()
    {
        position = transform.position;

        if (inWorld)
        {
            if (isZombie)
            {
                if (target != null)
                {
                    //if target is in the world, a zombie, and had a target, pursue it
                    Pursue(target);
                }
                else
                {
                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        //otherwise, wander
                        Wander();
                    }
                }
            }
            else
            {
                if (target != null)
                {
                    //if the target is in the world, a human, and has a target, evade it
                    Evade(target);
                }
                else
                {
                    if (Random.Range(0f, 1f) > 0.9f)
                    {
                        //otherwise, wander
                        Wander();
                    }
                }
            }
        }
        else
        {
            //if outside the world, return to center
            ReturnToCenter();
        }
        //keeps separate from same objects and avoids obstacles
        Separate();
        ObstacleAvoidance();

        //applies movement vectors to simulate movement
        velocity += acceleration * Time.deltaTime;
        if(velocity.magnitude > maxSpeed)
        {
            Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        position += velocity * Time.deltaTime;
        acceleration = Vector3.zero;
        direction = velocity.normalized;
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
    void Pursue(GameObject target)
    {
        Vector3 targetPosition = target.transform.position + (target.GetComponent<Movement>().Velocity.normalized * 5f);
        Vector3 desiredVelocity = targetPosition - position;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
        ApplyForce(steeringForce);
    }

    //creates and follows a steering force directed away from an object's future position
    void Evade(GameObject target)
    {
        Vector3 targetPosition = target.transform.position + (target.GetComponent<Movement>().Velocity.normalized * 5f);
        Vector3 desiredVelocity = position - targetPosition;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        Vector3 steeringForce = desiredVelocity - velocity;
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

    //determines where an object within a certain radius is and applies a force away from it
    void ObstacleAvoidance()
    {
        Vector3 steeringForce = Vector3.zero;
        if(obstacles.Count > 0)
        {
            foreach(GameObject obs in obstacles)
            {
                Vector3 dist = obs.transform.position - position;
                if (dist.magnitude < 7.5f)
                {
                    if(Vector3.Dot(dist,transform.right) > 0)
                    {
                        if(Vector3.Dot(dist, transform.forward) > 0)
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

    //creates randomized steeringforces in front of an object to simulate wandering
    void Wander()
    {
        Vector3 steeringForce = Random.insideUnitCircle * 75f;
        steeringForce.z = steeringForce.y;
        steeringForce += position + direction * 10f;
        steeringForce.y = 0;
        ApplyForce(steeringForce);
    }

    //checks if an object is within the world
    void CheckBounds()
    {
        if (position.x > worldSize.x - 25 || position.x < 25 || position.z > worldSize.z - 25 || position.z < 25)
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
        Vector3 center = new Vector3(worldSize.x / 2 - position.x, 0, worldSize.z / 2 - position.z);
        center.Normalize();
        center *= maxSpeed;
        ApplyForce(center);
    }

    //draws teh debug lines
    void OnRenderObject()
    {
        if (lines)
        {
            //velocity
            GL.PushMatrix();
            gameManager.matGreen.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(position);
            GL.Vertex(position + transform.right * 5.0f);
            GL.End();

            //right vector
            gameManager.matBlue.SetPass(0);
            GL.Begin(GL.LINES);
            GL.Vertex(position);
            GL.Vertex(position + transform.forward * -5.0f);
            GL.End();


            //To target
            if (null != target)
            {
                gameManager.matBlack.SetPass(0);
                GL.Begin(GL.LINES);
                GL.Vertex(position);
                GL.Vertex(target.transform.position);
                GL.End();
            }

            //future pos
            if (isZombie)
            {
                gameManager.matRed.SetPass(0);
            }
            else
            {
                gameManager.matPurple.SetPass(0);
            }
            GL.Begin(GL.LINES);
            for (int i = 0; i < 10; i++)
            {
                float a = i / 10;
                float angle = a * Mathf.PI * 2;
                GL.Vertex(position + direction * 7f);
                GL.Vertex(position + direction * 7.3f);
            }
            GL.End();
            GL.PopMatrix();
        }
    }
}

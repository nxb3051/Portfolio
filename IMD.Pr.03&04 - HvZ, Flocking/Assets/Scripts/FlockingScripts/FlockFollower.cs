using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlockFollower : MonoBehaviour {

    //stores objects to modify or use
    private Camera cam;
    public List<GameObject> flock;

    //helps with debug line
    Vector3 right;

    //stores the game manager
    private FlockGameManager gameManager;

	// Use this for initialization
	void Start () {
        //stores the game manager
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<FlockGameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        //finds the average position and direction of all flockers
        Vector3 position = Vector3.zero;
        Vector3 forward = Vector3.zero;
        right = Vector3.zero;
        if (flock.Count > 0)
        {
            foreach(GameObject obj in flock)
            {
                position += obj.transform.position;
                forward += obj.transform.forward;
                right += obj.transform.right;
            }
            position /= flock.Count;
        }
        //sets this object to the averages of everything
        this.transform.position = position;
        this.transform.forward = forward;
        this.transform.right = right;
	}

    //used to set the flock list through scripting
    public void setFlock(List<GameObject> flocker)
    {
        flock = flocker;
    }

    //used to set the camera through scripting, and also set the camera's target
    public void setCam(Camera cam)
    {
        this.cam = cam;
        this.cam.GetComponent<CameraFollow>().SetTarget(this.GetComponentInParent<Transform>());
    }
    
    //draws the debug lines
    void OnRenderObject()
    {
        //velocity
        GL.PushMatrix();
        gameManager.matPurple.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Vertex(transform.position);
        GL.Vertex(transform.position + right);
        GL.End();
        GL.PopMatrix();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
    // Camera array that holds a reference to every camera in the scene
    public Camera[] cameras;

    // Current camera 
    private int currentCameraIndex;

    //stores the text to be modified
    public Text tex;
    
    // Use this for initialization
    void Start ()
    { 

        currentCameraIndex = 0;

        // Turn all cameras off, except the first default one
        for (int i=1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }

        // If any cameras were added to the controller, enable the first one
        if (cameras.Length > 0)
        {
            cameras [0].gameObject.SetActive (true);
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        //modifies the text with information on the current perspective
        if (tex.text != null)
        {
            tex.text = "Press 'c' to change camera views\nCamera: " + (currentCameraIndex+1) + "\n";
        }
        if(currentCameraIndex == 0)
        {
            tex.text += "Overview of Terrain";
        }
        else if(currentCameraIndex == 1)
        {
            tex.text += "Leaders";
        }
        else if(currentCameraIndex == 2)
        {
            tex.text += "Midview of Horde";
        }
        else if(currentCameraIndex == 3)
        {
            tex.text += "Sideview of Horde";
        }
        else if(currentCameraIndex == 4)
        {
            tex.text += "Sideview of Terrain";
        }
        else
        {
            tex.text += "First Person Controller";
        }

        // Press the 'C' key to cycle through cameras in the array
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Cycle to the next camera
            currentCameraIndex++;

            // If cameraIndex is in bounds, set this camera active and last one inactive
            if (currentCameraIndex < cameras.Length)
            {
                cameras[currentCameraIndex-1].gameObject.SetActive(false);
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }

            // If last camera, cycle back to first camera
            else
            {
                cameras[currentCameraIndex-1].gameObject.SetActive(false);
                currentCameraIndex = 0;
                cameras[currentCameraIndex].gameObject.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        //tries to deal with when the camera falls through the terrain
        if (cameras[cameras.Length - 1].GetComponentInParent<Transform>().position.y < 0)
        {
            cameras[cameras.Length - 1].transform.position += new Vector3(0, 70f, 0);
        }
    }
}

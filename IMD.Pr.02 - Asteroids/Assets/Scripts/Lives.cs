using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lives : MonoBehaviour {
    //handles player lives and also takes care of end-game processes

    //stores all necessary components
    public GameObject text;
    public GameObject gg;
    public Button exit;
    public Button play;
    public GameObject player;

    int lives;

	// Use this for initialization
	void Start () {
        //sets lives count, deactivates buttons and sets their click events
        lives = 3;
        play.onClick.AddListener(Restart);
        exit.onClick.AddListener(Quit);
        play.gameObject.SetActive(false);
        exit.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void ChangeLife(int num)
    {
        //changes life count according to num
        lives += num;
        text.GetComponent<Text>().text = lives.ToString();
        //what happens when player lives equal 0
        if (lives == 0)
        {
            //destroys player and activates buttons
            gg.GetComponent<Text>().enabled = true;
            GameObject.FindGameObjectWithTag("Collision Manager").GetComponent<Collisions>().enabled = false;
            play.gameObject.SetActive(true);
            exit.gameObject.SetActive(true);
            Destroy(player);
        }
    }

    //called by the restart button, reloads the scene
    public void Restart()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index);
    }

    //exits the application on button click
    public void Quit()
    {
        Application.Quit();
    }
}

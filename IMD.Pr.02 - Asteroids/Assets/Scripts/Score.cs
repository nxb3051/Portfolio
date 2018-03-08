using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    //used exclusively for score keeping

    //holds the score displaying object and the score
    public GameObject text;
    int score;

	// Use this for initialization
	void Start () {
        score = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //can be called to increase the score
    public void RaiseScore(int points)
    {
        score += points;
        text.GetComponent<Text>().text = score.ToString();
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TrackBallOnScreen : MonoBehaviour {

	private GameObject 		ball;
	private LevelManager 	lManager;
	private Button 			button;
	private BungeeGum		bGum;

	// Use this for initialization
	void Start () {
		lManager = GameObject.Find("GameManager").GetComponent<LevelManager>();
		ball = GameObject.FindGameObjectWithTag ("Ball");
		bGum = GameObject.Find ("BungeeGum").GetComponent<BungeeGum> ();
//		button = GetComponent<Button> ();
//		button.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!lManager.levelComplete) {
			transform.position = Camera.main.WorldToScreenPoint (ball.transform.position);
		}
			
	}

	public void FindNewBall(GameObject newBall){
		ball = newBall;
	}
}

using UnityEngine;
using System.Collections;

public class BallEmitter : MonoBehaviour {

	GameObject dragBallButton;

	// Use this for initialization
	void Start () {
		dragBallButton = GameObject.Find ("DragBallButton");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NewBall(){
		GameObject ball = Instantiate (Resources.Load ("Ball")) as GameObject;
		ball.name = "Ball";
		dragBallButton.GetComponent<TrackBallOnScreen> ().FindNewBall (ball);
	}

	public void NewBall(float t){
		Invoke ("NewBall", t);
	}
}

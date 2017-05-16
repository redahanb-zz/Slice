using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

	BallEmitter bEmitter;
	GameObject 	dragBallButton;

	// Use this for initialization
	void Start () {
		bEmitter = GameObject.Find ("Emitter").GetComponent<BallEmitter> ();
		dragBallButton = GameObject.Find ("DragBallButton");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D c){
		if (c.gameObject.tag == "Recycler") {
			bEmitter.NewBall ();
			Destroy (gameObject);
		} 
		else if (c.gameObject.tag == "BungeeGum") {
			Vector3[] endPoints = c.gameObject.GetComponent<BungeeGum> ().GetEndPoints ();
			float stretchRange = Vector3.Distance (endPoints [0], endPoints [1]) * 0.3f;

			c.gameObject.GetComponent<EdgeCollider2D> ().enabled = false;
			c.gameObject.GetComponent<BungeeGum> ().DeformLine ();

			GameObject spring1 = new GameObject ();
			spring1.name = "AnchorPoint";
			spring1.transform.position = transform.position;
			spring1.AddComponent<SpringJoint2D> ();
			spring1.GetComponent<SpringJoint2D> ().connectedBody = gameObject.GetComponent<Rigidbody2D> ();
			spring1.GetComponent<SpringJoint2D> ().dampingRatio = 0.6f;
			spring1.GetComponent<SpringJoint2D> ().frequency = 4.0f;
			spring1.GetComponent<SpringJoint2D> ().distance = stretchRange;
			spring1.GetComponent<Rigidbody2D> ().isKinematic = true;
		} 
		else if (c.gameObject.tag == "Exit") {
			Destroy (gameObject);
			GameObject.Find ("GameManager").GetComponent<LevelManager> ().levelComplete = true;
			GameObject.Find ("LineField").GetComponent<LineField> ().ResetLine ();
			GameObject.Find ("BungeeGum").GetComponent<BungeeGum> ().ResetLine ();
			GameObject.Find ("InertiaField").GetComponent<LineField> ().ResetLine ();
			print ("Level Complete!");
		} 
		else if (c.gameObject.tag == "Wall") {
			Destroy (gameObject);
			bEmitter.NewBall ();
			//bEmitter.NewBall (1.0f);
		}
	}
}

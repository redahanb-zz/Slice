using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour {

	public 		bool 			isDragging;
	public 		bool 			launched;
	public 		GameObject 		lineField;
	public 		GameObject 		inertiaField;
	public 		GameObject 		bungeeGum;

	private 	int 			touchID;
	private 	float 			maxStretch = 3.0f;
	private 	float 			maxStretchSq;
	private 	Vector3			touchPosition;
	private 	Vector2 		prevVelocity;
	private		LevelManager	lManager;		
	private 	Ray				mouseToBungee;
	private 	LineRenderer 	lRenderer;
	private 	Vector3 		mouseInWorldSpace, tempV3;
	private 	enum 			ActivePower
								{
									LineField, 
									BungeeGem, 
									InertiaField,
								};
	private 	ActivePower 	activePower;

	void Start () 
	{
		activePower = ActivePower.LineField;
		maxStretchSq = Mathf.Pow(maxStretch,2);
		lManager = GameObject.Find ("GameManager").GetComponent<LevelManager> ();
	} // end Start
	

	void Update () 
	{
		FindNewTouch ();
		GetOldTouch ();
		print (EventSystem.current.IsPointerOverGameObject (touchID));

		if (!isDragging && !lManager.levelComplete) {
			GameObject ball = GameObject.Find ("Ball");
			ball.GetComponent<Rigidbody2D> ().isKinematic = false;
		}
			
		if (GameObject.Find ("AnchorPoint")) {
			if (launched && prevVelocity.sqrMagnitude > GameObject.Find ("Ball").GetComponent<Rigidbody2D> ().velocity.sqrMagnitude) {
				launched = false;
				Destroy (GameObject.Find ("AnchorPoint"));
				bungeeGum.GetComponent<BungeeGum> ().ResetLine ();
				GameObject.Find ("Ball").GetComponent<Rigidbody2D> ().velocity = prevVelocity;
			}

			if(launched){
				prevVelocity = GameObject.Find ("Ball").GetComponent<Rigidbody2D> ().velocity;
			}
		}

		switch (activePower)
		{
		case ActivePower.LineField:
			lRenderer = lineField.GetComponent<LineRenderer> ();
			if (!isDragging) {
				LineField ();
			}
			break;
		case ActivePower.BungeeGem:
			lRenderer = bungeeGum.GetComponent<LineRenderer> ();
			if (!isDragging) {
				BungeeGum ();
			}
			break;
		case ActivePower.InertiaField:
			lRenderer = inertiaField.GetComponent<LineRenderer> ();
			if (!isDragging) {
				InertiaField ();
			}
			break;
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			SwitchActivePower ();
		}
		
			
	} // end Update

	void LineField(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			if (Input.GetMouseButtonDown (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				//lRenderer.GetComponent<LineField> ().StartLine (tempV3);
				lineField.GetComponent<LineField> ().StartLine (tempV3);
			} else if (Input.GetMouseButton (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				//lRenderer.GetComponent<LineField> ().StretchLine (tempV3);
				lineField.GetComponent<LineField> ().StretchLine (tempV3);
			} else if (Input.GetMouseButtonUp (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				//lRenderer.GetComponent<LineField> ().FinishLine (tempV3);
				lineField.GetComponent<LineField> ().FinishLine (tempV3);
			}
		}

		if(!EventSystem.current.IsPointerOverGameObject(touchID)){
			if (Input.touchCount > 0 && !isDragging) {
				if (FindNewTouch ()) {
					lineField.GetComponent<LineField> ().StartLine (touchPosition);
				} else if (Input.touches [touchID].phase == TouchPhase.Moved || Input.touches[touchID].phase == TouchPhase.Stationary) {
					lineField.GetComponent<LineField> ().StretchLine (touchPosition);
				} else if (Input.touches [touchID].phase == TouchPhase.Ended) {
					lineField.GetComponent<LineField> ().FinishLine (touchPosition);
				}
			}
		}
		
	}

	void InertiaField(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			if (Input.GetMouseButtonDown (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				//lRenderer.GetComponent<LineField> ().StartLine (tempV3);
				inertiaField.GetComponent<LineField> ().StartLine (tempV3);
			} else if (Input.GetMouseButton (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				//lRenderer.GetComponent<LineField> ().StretchLine (tempV3);
				inertiaField.GetComponent<LineField> ().StretchLine (tempV3);
			} else if (Input.GetMouseButtonUp (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				//lRenderer.GetComponent<LineField> ().FinishLine (tempV3);
				inertiaField.GetComponent<LineField> ().FinishLine (tempV3);
			}
		}

		if(!EventSystem.current.IsPointerOverGameObject(touchID)){
			if (Input.touchCount > 0 && !isDragging) {
				if (FindNewTouch ()) {
					inertiaField.GetComponent<LineField> ().StartLine (touchPosition);
				} else if (Input.touches [touchID].phase == TouchPhase.Moved || Input.touches[touchID].phase == TouchPhase.Stationary) {
					inertiaField.GetComponent<LineField> ().StretchLine (touchPosition);
				} else if (Input.touches [touchID].phase == TouchPhase.Ended) {
					inertiaField.GetComponent<LineField> ().FinishLine (touchPosition);
				}
			}
		}

	}

	void BungeeGum(){
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			if (Input.GetMouseButtonDown (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				lRenderer.GetComponent<BungeeGum> ().StartLine (tempV3);
				launched = false;
			} else if (Input.GetMouseButton (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				lRenderer.GetComponent<BungeeGum> ().StretchLine (tempV3);
			} else if (Input.GetMouseButtonUp (0)) {
				mouseInWorldSpace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				tempV3 = new Vector3 (mouseInWorldSpace.x, mouseInWorldSpace.y, 0);
				lRenderer.GetComponent<BungeeGum> ().FinishLine (tempV3);
			}
		}

		if(!EventSystem.current.IsPointerOverGameObject(touchID)){
			print (isDragging);
			if (Input.touchCount > 0 && !isDragging) {
				print ("Not over gameObject!");
				if (FindNewTouch ()) {
					bungeeGum.GetComponent<BungeeGum> ().StartLine (touchPosition);
				} else if (Input.touches [touchID].phase == TouchPhase.Moved || Input.touches[touchID].phase == TouchPhase.Stationary) {
					bungeeGum.GetComponent<BungeeGum> ().StretchLine (touchPosition);
				} else if (Input.touches [touchID].phase == TouchPhase.Ended) {
					bungeeGum.GetComponent<BungeeGum> ().FinishLine (touchPosition);
				}
			}
		}
	}

	public void SwitchActivePower(){
		if (activePower == ActivePower.LineField) {
			activePower = ActivePower.BungeeGem;
		} 
		else if (activePower == ActivePower.BungeeGem) {
			activePower = ActivePower.LineField;
		}
	}

	public void SetPowerLine(){
		activePower = ActivePower.LineField;
	}

	public void SetPowerBungee(){
		activePower = ActivePower.BungeeGem;
	}

	public void SetPowerInertia(){
		activePower = ActivePower.InertiaField;
	}

	public void DragBall(){
		print ("Drag ball called..");
		if (bungeeGum.GetComponent<BungeeGum> ().attached) {
			isDragging = true;
			prevVelocity = Vector2.zero;
			GameObject ball = GameObject.Find ("Ball");
			ball.GetComponent<Rigidbody2D> ().isKinematic = true;
			Vector3 temp = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (Input.touchCount > 0) {
				//FindNewTouch ();
				GetOldTouch ();
				temp = touchPosition;
			}
			temp.z = 0;

			mouseToBungee = new Ray (GameObject.Find ("AnchorPoint").transform.position, Vector2.zero);
			Vector2 dir = temp - GameObject.Find ("AnchorPoint").transform.position;

			if (dir.sqrMagnitude > maxStretchSq) {
				mouseToBungee.direction = dir;
				ball.transform.position = mouseToBungee.GetPoint (maxStretch);
			} else {
				ball.transform.position = temp;
			}
			//ball.GetComponent<SpringJoint2D> ().enabled = false;
			GameObject.Find ("AnchorPoint").GetComponent<SpringJoint2D> ().enabled = false;
		}
	}

	public void ReleaseBall(){
		print ("Release ball called..");
		if (bungeeGum.GetComponent<BungeeGum> ().attached) {
			isDragging = false;
			launched = true;
			GameObject ball = GameObject.Find ("Ball");
			ball.GetComponent<Rigidbody2D> ().isKinematic = false;
			//ball.GetComponent<SpringJoint2D> ().enabled = true;
			GameObject.Find ("AnchorPoint").GetComponent<SpringJoint2D> ().enabled = true;
		}
	}

	bool FindNewTouch(){
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				touchID = touch.fingerId;
				touchPosition = Camera.main.ScreenToWorldPoint (touch.position);
				touchPosition.z = 0;
				return true;
			}
		}
		return false;
	}

	void GetOldTouch(){
		foreach (Touch touch in Input.touches) {
			if (touch.fingerId == touchID) {
				touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
				touchPosition.z = 0;
			}
		}
	}

	public int NewTouch(){
		return touchID;
	}

	public Vector3 TouchPosition(){
		return touchPosition;
	}

}

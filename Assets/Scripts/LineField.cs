using UnityEngine;
using System.Collections;

public class LineField : MonoBehaviour {

	public 	float 				maxLength;

	private GameObject 			objectPool;
	private LineRenderer 		lRenderer;
	private EdgeCollider2D 		edge;
	private Vector3 			edge1, edge2;
	private PlayerController 	pController;

	private Ray 				startToEnd;

	// Use this for initialization
	void Start () {
		objectPool 	= GameObject.Find ("ObjectPool");
		lRenderer 	= GetComponent<LineRenderer> ();
		edge 		= GetComponent<EdgeCollider2D> ();
		pController	= GameObject.Find ("Player").GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void StartLine(Vector3 start){
		CancelInvoke ("ResetLine");
		ResetLine ();

		transform.position = start;
		lRenderer.SetPosition (0, start);
		lRenderer.SetPosition (1, start);

		edge.points = new Vector2[]{objectPool.transform.position, objectPool.transform.position};
		edge1 = start;

		gameObject.transform.position = start;
		transform.GetChild (0).GetComponent<Animation> ().Play ();
	}

	public void StretchLine(Vector3 v3){
		startToEnd = new Ray (edge1, Vector2.zero);
		Vector2 dir = v3 - edge1;

		if (dir.sqrMagnitude > Mathf.Pow (maxLength, 2)) {
			print ("Restricting..");
			startToEnd.direction = dir;
			Vector3 tempV3 = startToEnd.GetPoint (maxLength);
			lRenderer.SetPosition (1, tempV3);
			transform.GetChild (1).transform.position = tempV3;
		} else {
			print ("Unrestricted stretch..");
			lRenderer.SetPosition (1, v3);
			transform.GetChild (1).transform.position = v3;
		}

	}

	public void FinishLine(Vector3 end){
		transform.GetChild (1).GetComponent<Animation> ().Play ();
		startToEnd = new Ray (edge1, Vector2.zero);
		Vector2 dir = end - edge1;

		if(dir.sqrMagnitude > Mathf.Pow(maxLength,2)){
			startToEnd.direction = dir;
			edge2 = startToEnd.GetPoint (maxLength) - edge1;
		} else{
			edge2 = end - edge1;
		}
			
		edge.points = new Vector2[]{ Vector3.zero, edge2 };
		Invoke ("ResetLine", 2.0f);
	}

	public void ResetLine(){
		if (Input.GetMouseButton (0)) {
			return;
		}
		lRenderer.SetPosition (0, objectPool.transform.position);
		lRenderer.SetPosition (1, objectPool.transform.position);
		transform.position = objectPool.transform.position;
		edge.points = new Vector2[]{objectPool.transform.position, objectPool.transform.position};
	}
}

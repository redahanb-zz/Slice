using UnityEngine;
using System.Collections;

public class BungeeGum : MonoBehaviour  {

	public 	float 				maxLength;
	public 	bool 				attached;

	private GameObject 			objectPool;
	private LineRenderer 		lRenderer;
	private EdgeCollider2D 		edge;
	private Vector3 			edge1, edge2;
	private Ray 				startToEnd;

	// Use this for initialization
	void Start () {
		objectPool = GameObject.Find ("ObjectPool");
		lRenderer 	= GetComponent<LineRenderer> ();
		edge 		= GetComponent<EdgeCollider2D> ();
	}


	void Update () {

	}

	void LateUpdate(){
		if (attached) {
			lRenderer.SetPosition(1, GameObject.Find("Ball").transform.position);
		}
	}

	public void StartLine(Vector3 start){
		CancelInvoke ("ResetLine");
		ResetLine ();

		lRenderer.transform.position = start;
		lRenderer.SetPosition (0, start);
		lRenderer.SetPosition (1, start);

		edge.points = new Vector2[]{objectPool.transform.position, objectPool.transform.position};
		edge1 = start;

		gameObject.transform.position = start;
	}

	public void StretchLine(Vector3 v3){
		startToEnd = new Ray (edge1, Vector2.zero);
		Vector2 dir = v3 - edge1;
		print (dir.sqrMagnitude);

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

		//lRenderer.SetPosition (1, v3);
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

		//edge2 = end - edge1;
		//edge.points = new Vector2[]{ Vector3.zero, edge2 };
		//Invoke ("ResetLine", 2.0f);
	}

	public void ResetLine(){
		attached = false;
		lRenderer.SetVertexCount (2);
		lRenderer.SetPosition (0, objectPool.transform.position);
		lRenderer.SetPosition (1, objectPool.transform.position);
		transform.position = objectPool.transform.position;
		edge.enabled = true;
		edge.points = new Vector2[]{objectPool.transform.position, objectPool.transform.position};
		Destroy(GameObject.Find("AnchorPoint"));
		Destroy (GameObject.Find ("Ball").GetComponent<SpringJoint2D> ());
	}

	public void DeformLine(){
		attached = true;
		lRenderer.SetVertexCount (3);
		lRenderer.SetPosition (2, edge2 + edge1);
		lRenderer.SetPosition(1, GameObject.Find("Ball").transform.position);
	}

	public Vector3[] GetEndPoints(){
		return new Vector3[]{edge1, edge1 + edge2};
	}
}

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public 	bool 				levelComplete;
	public 	GameObject 			endText;
	public 	GameObject 			levelButton;
	public 	GameObject			player;

	private PlayerController 	pController;
	private GameManager	gManager;

	// Use this for initialization
	void Start () {
		gManager = levelButton.GetComponent<GameManager> ();
		pController = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (levelComplete) {
			endText.SetActive (true);
			levelButton.SetActive (true);
			pController.enabled = false;
			Time.timeScale = 0.0f;
		}


	}
}

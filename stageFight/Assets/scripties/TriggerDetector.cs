using UnityEngine;
using System.Collections;

public class TriggerDetector : MonoBehaviour {

	private GameManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("Camera").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider myTrigger) {
		if (manager.isGamePlaying ()) {
			if (myTrigger.gameObject.CompareTag ("Player")) {
				if (myTrigger.gameObject.name == "Player red") {
					manager.endGame (2);
				} else {
					manager.endGame (1);
				}
			}
		}
	}
}

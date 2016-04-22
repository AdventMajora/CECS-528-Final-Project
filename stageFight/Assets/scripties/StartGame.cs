using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	public void StartScene(){
		Application.LoadLevel ("StageFight");
	}

	public void ExitGame(){
		Application.Quit ();
	}


}

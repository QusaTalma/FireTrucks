using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {
	
	public void StartClicked(){
		Application.LoadLevel ("LevelMenu");
	}

}
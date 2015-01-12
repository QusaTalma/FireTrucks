using UnityEngine;
using System.Collections;

public class TitleScript : MonoBehaviour {
	const float TIME_TO_SHOW = 1.5f;
	private float timeOnScreen = 0;

	void Start(){
		timeOnScreen = 0;
	}
	
	// Update is called once per frame
	void Update () {
		timeOnScreen += Time.deltaTime;

		if (timeOnScreen >= TIME_TO_SHOW){
			Application.LoadLevel("LevelMenu");
		}
	}
}

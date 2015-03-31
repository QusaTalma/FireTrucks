using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class SplashController : MonoBehaviour {
	const float TIME_TO_SHOW = 2f;
	private float timeOnScreen = 0;
	
	public string nextScene;
	
	void Start(){
		timeOnScreen = 0;
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = true;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
	}
	
	// Update is called once per frame
	void Update () {
		timeOnScreen += Time.deltaTime;
		
		if (timeOnScreen >= TIME_TO_SHOW){
			Application.LoadLevel(nextScene);
		}
	}
}

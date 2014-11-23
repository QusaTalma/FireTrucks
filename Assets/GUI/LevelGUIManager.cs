using UnityEngine;
using System.Collections;

public class LevelGUIManager : MonoBehaviour {
	public GUIStyle labelStyle;
	public GUIStyle winLabelStyle;
	public GUIStyle loseLabelStyle;
	public GUIStyle restartButtonStyle;

	EGDispatcher _dispatcher;
	TGMap _map;

	const float LABEL_HEIGHT = 50;
	const float LABEL_WIDTH = 250;
	const float TRUCK_COUNT_TOP = 20;
	const float CITY_HEALTH_TOP = TRUCK_COUNT_TOP + LABEL_HEIGHT + 1;
	const float TIME_REMAINING_TOP = CITY_HEALTH_TOP + LABEL_HEIGHT + 1;

	private float xStart;

	void Start(){
		_dispatcher = gameObject.GetComponent<EGDispatcher> ();
		_map = gameObject.GetComponent<TGMap> ();
		xStart = (Screen.height / 2f) - LABEL_HEIGHT * 2;
	}

	void OnGUI(){
		DrawTruckCount ();
		DrawCityHealth ();
		DrawTimeRemaining ();

		if (!_map.GetGameSession ().IsActive ()) {
			if(_map.GetCityDurabilityPercent() >= _map.PercentToWin){
				DrawWinLabel();
			}else{
				DrawLoseLabel();
			}

			if(DrawMenuButton()){
				Application.LoadLevel("LevelMenu");
				Time.timeScale = 1f;
			}

			if(DrawRestartButton()){
				Application.LoadLevel(Application.loadedLevel);
				Time.timeScale = 1f;
			}

			string nextLevel = LevelManager.Instance.GetNextLevel();

			if(nextLevel != null){
				if(DrawNextButton()){
					LevelManager.Instance.CurrentLevel = nextLevel;
					Application.LoadLevel("GamePlay");
					Time.timeScale = 1f;
				}
			}
		}
	}

	void DrawTruckCount(){
		float left, top, width, height;
		top = TRUCK_COUNT_TOP;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		left = Screen.width - width;
		string truckCount = _dispatcher.GetTruckCount () + " Firetrucks in play";
		GUI.Label (new Rect (left, top, width, height), truckCount, labelStyle); 
	}

	void DrawCityHealth(){
		float left, top, width, height;
		top = CITY_HEALTH_TOP;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		left = Screen.width - width;
		float durabilityPercent = _map.GetCityDurabilityPercent ();
		string cityPercent = "Buildings remaining: " + durabilityPercent.ToString("P0") +
						   "\nRequired to win:     " + _map.PercentToWin.ToString("P0");
		GUI.Label (new Rect (left, top, width, height), cityPercent, labelStyle); 
	}

	void DrawTimeRemaining(){
		float left, top, width, height;
		top = TIME_REMAINING_TOP;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		left = Screen.width - width;
		float timeRemaining = _map.GetGameSession ().GetRemainingTime ();
		string cityPercent = "Time remaining: " + timeRemaining.ToString("F0");
		GUI.Label (new Rect (left, top, width, height), cityPercent, labelStyle); 
	}

	void DrawWinLabel(){
		float left, top, width, height;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		top = xStart - height/2f;
		left = (Screen.width/2f) - width/2f;
		string winText = "YOU WIN";
		GUI.Label (new Rect (left, top, width, height), winText, winLabelStyle); 
	}

	void DrawLoseLabel(){
		float left, top, width, height;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		top = xStart - height/2f;
		left = (Screen.width/2f) - width/2f;
		string loseText = "YOU LOSE";
		GUI.Label (new Rect (left, top, width, height), loseText, loseLabelStyle); 
	}
	
	bool DrawMenuButton(){
		float left, top, width, height;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		top = xStart - height/2f + height;
		left = (Screen.width/2f) - width/2f;
		string menuText = "MENU";
		return GUI.Button (new Rect (left, top, width, height), menuText, restartButtonStyle);
	}

	bool DrawRestartButton(){
		float left, top, width, height;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		top = xStart - height/2f + height*2;
		left = (Screen.width/2f) - width/2f;
		string restartText = "RESTART";
		return GUI.Button (new Rect (left, top, width, height), restartText, restartButtonStyle);
	}

	bool DrawNextButton(){
		float left, top, width, height;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		top = xStart - height/2f + height*3;//Go under the restart button
		left = (Screen.width/2f) - width/2f;
		string nextText = "NEXT";
		return GUI.Button (new Rect (left, top, width, height), nextText, restartButtonStyle);
	}
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGUIManager : MonoBehaviour {
	public GameObject endGameDialog;
	public Button nextLevelButton;
	public Text winText;
	public Text loseText;
	public Text truckCount;
	public Text timeRemaining;
	public Text currentPercent;
	public Text winPercent;

	EGDispatcher _dispatcher;
	TGMap _map;

	const float LABEL_HEIGHT = 50;
	const float LABEL_WIDTH = 250;
	const float TRUCK_COUNT_TOP = 20;
	const float CITY_HEALTH_TOP = TRUCK_COUNT_TOP + LABEL_HEIGHT + 1;
	const float TIME_REMAINING_TOP = CITY_HEALTH_TOP + LABEL_HEIGHT + 1;

	void Start(){
		_dispatcher = gameObject.GetComponent<EGDispatcher> ();
		_map = gameObject.GetComponent<TGMap> ();
		endGameDialog.gameObject.SetActive (false);
	}

	public void ShowEndGameDialog(){
		endGameDialog.gameObject.SetActive (true);

		if (_map.GetCityDurabilityPercent () >= _map.PercentToWin) {
			loseText.gameObject.SetActive (false);
			winText.gameObject.SetActive (true);
			
			string nextLevel = LevelManager.Instance.GetNextLevel();
			nextLevelButton.gameObject.SetActive (nextLevel != null);
		} else {
			loseText.gameObject.SetActive (true);
			winText.gameObject.SetActive (false);
			nextLevelButton.gameObject.SetActive(false);
		}
	}

	void OnGUI(){
		DrawTruckCount ();
		DrawCityHealth ();
		DrawTimeRemaining ();
	}

	public void RestartClicked(){
		Application.LoadLevel(Application.loadedLevel);
		Time.timeScale = 1f;
	}

	public void MenuClicked(){
		Application.LoadLevel("LevelMenu");
		Time.timeScale = 1f;
	}

	public void NextClicked(){
		string nextLevel = LevelManager.Instance.GetNextLevel();

		LevelManager.Instance.CurrentLevel = nextLevel;
		Application.LoadLevel("GamePlay");
		Time.timeScale = 1f;
	}

	void DrawTruckCount(){
		truckCount.text = _dispatcher.GetTruckCount ().ToString ();
	}

	void DrawCityHealth(){
		currentPercent.text = _map.GetCityDurabilityPercent ().ToString ("P0");
		winPercent.text = _map.PercentToWin.ToString ("P0");
	}

	void DrawTimeRemaining(){
		timeRemaining.text = _map.GetGameSession ().GetRemainingTime ().ToString ("F0");
	}
}
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
	public GameObject statusPanel;

	EGDispatcher _dispatcher;
	TGMap _map;
	
	private static LevelGUIManager _instance = null;
	
	public static LevelGUIManager Instance{
		get {
			if (_instance == null) {
				_instance = new LevelGUIManager ();
			}
			
			return _instance;
		}
	}

	void Start(){
		_instance = this;
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
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelGUIManager : MonoBehaviour {
	public GameObject endGameDialog;
	public Button nextLevelButton;
	public Text winText;
	public Text loseText;
	public GameObject statusPanel;
	public Image thermometerRed;
	public Text flameCountLabel;

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

		bool levelWon = _map.GetCityDurabilityPercent () >= _map.PercentToWin;

		if (levelWon) {
			int nextindex = LevelManager.Instance.GetIndexForLevel(LevelManager.Instance.CurrentLevel)+1;
			LevelManager.Instance.UnlockLevel(nextindex);
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

	void Update(){
		UpdateThermometer ();
		UpdateFlameCountLabel ();
	}

	void UpdateThermometer(){
		//Get current and minimum %s
		float current = _map.GetCityDurabilityPercent ();
		float minimum = _map.PercentToWin;

		//Convert the percents to a value out of 1
		float safeRange = 1f - minimum;
		float burnedRange = 1f - current;

		//Use that ratio to calculate the fill amount
		float fillAmount = burnedRange / safeRange;
		thermometerRed.fillAmount = fillAmount;
	}

	void UpdateFlameCountLabel(){
		GameObject[] fires = GameObject.FindGameObjectsWithTag("Fire");
		int flameCount = fires.Length;
		flameCountLabel.text = flameCount.ToString ();
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
}
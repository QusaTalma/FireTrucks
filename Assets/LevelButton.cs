using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButton : MonoBehaviour {
	public int levelIndex;
	public Text text;
	
	private Button button;

	void Start () {
		button = GetComponent<Button> ();
	}

	void Update () {
		string level = LevelManager.Instance.GetLevelAtIndex (levelIndex - 1);
		bool unlocked = levelIndex == 1 || LevelManager.Instance.IsLevelUnlocked(level);
		button.interactable = unlocked;
		if (unlocked) {
			text.text = levelIndex.ToString();
		} else {
			text.text = "";
		}
	}

	public void OnClick(){
		string level = LevelManager.Instance.GetLevelAtIndex (levelIndex-1);
		MenuGUIManager.LoadLevel (level);
	}
}
using UnityEngine;
using System.Collections;

public class MenuGUIManager : MonoBehaviour {
	public static void LoadLevel(string levelName){
		LevelManager.Instance.CurrentLevel = levelName;
		Application.LoadLevel("GamePlay");
	}
}
using System;
using UnityEngine;

public class LevelManager {
	private const string LOCK_KEY = "LockedLevel";
	private static LevelManager _instance = null;

	public static LevelManager Instance{
		get {
			if (_instance == null) {
				_instance = new LevelManager ();
				_instance.UnlockLevel(1);
			}
			
			return _instance;
		}
	}

	private LevelManager(){}

	readonly string[] LEVELS = {
		"FunInTheSun",
		"LostOfFunInTheSun",
		"MoreFunInTheSun",

		"SunBurn",
		"SunBurn2",
		"SunBurn3",
		"SunBurn4",

		"MyFirstLevel",
		"MySecondLevel",
		"MyThirdLevel",

		"Level1",
		"Level2",
		"Level3",
		"Level4",

		"test1",
		"test2",
		"test3",
		"test4"
	};

	public string[] Levels{
		get {
			return LEVELS;
		}
	}

	public int LevelCount(){
		return LEVELS.Length;
	}

	private string _currentLevel = "SunBurn";
	public string CurrentLevel{
		get{
			return _currentLevel;
		}

		set{
			_currentLevel = value;
		}
	}

	public string GetCurrentLevelFilePath(){
		string path = null;

		if (CurrentLevel != null) {
			path = "Levels/" + CurrentLevel;
		}

		return path;
	}

	public string GetNextLevel(){
		string nextLevel = null;

		int currentLevelIndex = GetIndexForLevel (CurrentLevel);
		if (currentLevelIndex > -1 && currentLevelIndex+1 < LevelCount()) {
			nextLevel = LEVELS[currentLevelIndex+1];
		}

		return nextLevel;
	}

	public string GetLevelAtIndex(int index){
		return LEVELS[index];
	}

	public int GetIndexForLevel(string level){
		return Array.IndexOf (LEVELS, level);
	}

	public void UnlockLevel(int index){
		string key = LOCK_KEY + index.ToString ();
		PlayerPrefs.SetInt (key, 1);
	}

	public bool IsLevelUnlocked(string level){
		int index = GetIndexForLevel (level);
		string key = LOCK_KEY + index.ToString();
		int lockValue = PlayerPrefs.GetInt (key, 0);
		Debug.Log (key + " is " + lockValue.ToString ());
		return lockValue == 1;
	}
}
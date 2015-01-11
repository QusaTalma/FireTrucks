using System;

public class LevelManager {
	private static LevelManager _instance = null;

	public static LevelManager Instance{
		get {
			if (_instance == null) {
				_instance = new LevelManager ();
			}
			
			return _instance;
		}
	}

	private LevelManager(){}

	readonly string[] LEVELS = {
		"test1",
		"test2",
		//"test3",
		"test4",
		"MyFirstLevel",
		"MySecondLevel",
		"MyThirdLevel"
	};

	public string[] Levels{
		get {
			return LEVELS;
		}
	}

	public int LevelCount(){
		return LEVELS.Length;
	}

	private string _currentLevel = "test4";
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

		int currentLevelIndex = Array.IndexOf (LEVELS, CurrentLevel);
		if (currentLevelIndex > -1 && currentLevelIndex+1 < LevelCount()) {
			nextLevel = LEVELS[currentLevelIndex+1];
		}

		return nextLevel;
	}
}
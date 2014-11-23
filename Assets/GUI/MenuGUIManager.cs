using UnityEngine;
using System.Collections;

public class MenuGUIManager : MonoBehaviour {

	public GUIStyle LevelButtonStyle;

	const float LEVEL_BUTTON_HEIGHT = 50;
	const float LEVEL_BUTTON_WIDTH = 50;
	const int LEVEL_BUTTON_PADDING = 20;

	const int COLUMN_COUNT = 4;

	void OnGUI() {
		int row = 0;
		int column = 0;
		int x = LEVEL_BUTTON_PADDING;
		int y = LEVEL_BUTTON_PADDING;

		string[] levels = LevelManager.Instance.Levels;
		for (int i=0; i<levels.Length; i++) {
			if(column >= COLUMN_COUNT){
				row++;
				x = LEVEL_BUTTON_PADDING;
				column = 0;
				y += LEVEL_BUTTON_PADDING + (int)LEVEL_BUTTON_HEIGHT;
			}

			string level = levels[i];

			if(GUI.Button(new Rect(x, y, LEVEL_BUTTON_WIDTH, LEVEL_BUTTON_HEIGHT), i.ToString(), LevelButtonStyle)){
				LevelManager.Instance.CurrentLevel = level;
				Application.LoadLevel("GamePlay");
			}

			x += LEVEL_BUTTON_PADDING + (int)LEVEL_BUTTON_WIDTH ;
		}
	}
}
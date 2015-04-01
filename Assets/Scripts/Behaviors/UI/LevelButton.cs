using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Managers;

namespace Behaviors.UI{
	public class LevelButton : MonoBehaviour {
		public int levelNumber;//1 indexed, not 0 indexed
		public Text text;
		
		private Button button;

		void Start () {
			button = GetComponent<Button> ();
		}

		void Update () {
			string level = LevelManager.Instance.GetLevelAtIndex (levelNumber - 1);
			bool unlocked = LevelManager.Instance.IsLevelUnlocked(level);
			button.interactable = unlocked;
			if (unlocked) {
				text.text = levelNumber.ToString();
			} else {
				text.text = "";
			}
		}

		public void OnClick(){
			string level = LevelManager.Instance.GetLevelAtIndex (levelNumber-1);
			MenuGUIManager.LoadLevel (level);
		}
	}
}
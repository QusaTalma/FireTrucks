using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;

namespace Controllers{
	public class MainMenuController : MonoBehaviour {
		
		public void StartClicked(){
			Application.LoadLevel ("LevelMenu");
		}

		public void CreditsClicked(){
			Application.LoadLevel ("Credits");
		}

		public void SignInClicked(){
			Social.localUser.Authenticate ((bool success) => {

			});
		}
	}
}
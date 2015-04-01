using UnityEngine;
using System.Collections;

namespace Controllers{
	public class CreditsController : MonoBehaviour {

		public void MenuClicked(){
			Application.LoadLevel ("MainMenu");
		}
	}
}
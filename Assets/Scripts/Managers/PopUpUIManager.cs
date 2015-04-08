using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Behaviors.EntityGraphics;
using DataStructure.UI;
using Behaviors.UI;

namespace Managers{
	public class PopUpUIManager : MonoBehaviour {
		public GameObject fireChiefImage;
		public GameObject policeChiefImage;
		public GameObject mayorImage;
		public GameObject popUpPanel;
		public Text alertMessage;
		public GameObject flameIndicatorPrefab;

		private List<PopUpEvent> popUpQueue = new List<PopUpEvent>();
		private bool showingAlert = false;
		private bool radioOn = true;
		public bool RadioOn{
			get { return radioOn; }
			set { radioOn = value;
				timeSinceMessage = 0f;
				HideAlert();
				popUpQueue = new List<PopUpEvent>(); }
		}

		public const float MESSAGE_DISPLAY_TIME = 3.5f;

		private const string FIRE_SPREAD_MESSAGE = "Fires are spreading! Get them under control!";
		private const string BUILDING_BURNED_DOWN_MESSAGE = "Oh dear, a building burned down!";
		private const string FIRE_START_MESSAGE = "Someone's starting fires in MY city!";
		private const string HALF_FINISHED_MESSAGE = "We're hot on his trail!";
		private const string ALMOST_FINISHED_MESSAGE = "We almost got him!";
		private const string FINISHED_MESSAGE = "We caught him!";

		private float timeSinceMessage = 0f;
		private float currentMessageDuration = 0f;
		
		private static PopUpUIManager _instance = null;
		
		public static PopUpUIManager Instance{
			get {
				if (_instance == null) {
					_instance = new PopUpUIManager ();
				}
				
				return _instance;
			}
		}

		void Start(){
			_instance = this;
			HideAlert ();
		}

		public void ToggleRadio(){
			RadioOn = !RadioOn;
		}

		public void ShowMayor(string message, float duration, bool force=false){
			if ((!showingAlert || force) && radioOn) {
				ShowNPCMessage(mayorImage, message, duration);
			} else {
				popUpQueue.Add(new PopUpEvent(message, duration, NPC.MAYOR));
			}
		}

		public void ShowBuildingBurnedDownMessage(){
			ShowMayor(BUILDING_BURNED_DOWN_MESSAGE, MESSAGE_DISPLAY_TIME);
		}

		public void ShowFireSpreadNPCMessage(){
			ShowFireChief(FIRE_SPREAD_MESSAGE, MESSAGE_DISPLAY_TIME);
		}
		
		public void ShowFireChief(string message, float duration, bool force=false){
			if ((!showingAlert || force) && radioOn) {
				ShowNPCMessage(fireChiefImage, message, duration);
			} else {
				popUpQueue.Add(new PopUpEvent(message, duration, NPC.FIRECHIEF));
			}
		}

		public void ShowFiresStartingMessage(){
			ShowPoliceChief (FIRE_START_MESSAGE, MESSAGE_DISPLAY_TIME);
		}

		public void ShowFiresHalfFinishedMessage(){
			ShowPoliceChief(HALF_FINISHED_MESSAGE, MESSAGE_DISPLAY_TIME);
		}

		public void ShowFiresAlmostFinishedMessage(){
			ShowPoliceChief (ALMOST_FINISHED_MESSAGE, MESSAGE_DISPLAY_TIME);
		}

		public void ShowFiresFinishedMessage(){
			ShowPoliceChief (FINISHED_MESSAGE, MESSAGE_DISPLAY_TIME);
		}

		public void ShowPoliceChief(string message, float duration, bool force=false){
			if ((!showingAlert || force) && radioOn) {
				ShowNPCMessage(policeChiefImage, message, duration);
			} else {
				popUpQueue.Add(new PopUpEvent(message, duration, NPC.POLICECHIEF));
			}
		}

		public void HideAlert(){
			mayorImage.SetActive (false);
			policeChiefImage.SetActive (false);
			fireChiefImage.SetActive (false);
			popUpPanel.SetActive (false);
			timeSinceMessage = MESSAGE_DISPLAY_TIME;
			showingAlert = popUpQueue.Count > 0;
		}

		public void ShowFlameIndicator(EGFlame flame){
			if (radioOn) {			
				GameObject indicatorObject = (GameObject)Instantiate (flameIndicatorPrefab);
				UIFlameIndicator flameIndicator = indicatorObject.GetComponent<UIFlameIndicator> ();
				if (flameIndicator != null) {
					flameIndicator.Flame = flame;
				}
			}
		}

		private void ShowNPCMessage(GameObject image, string message, float duration){
			HideAlert();
			showingAlert = true;
			currentMessageDuration = duration;
			timeSinceMessage = 0f;
			image.SetActive (true);
			alertMessage.text = message;
			popUpPanel.SetActive (true);
		}
		
		void Update(){
			if (timeSinceMessage <= currentMessageDuration && radioOn) {
				timeSinceMessage += Time.deltaTime;
				if(timeSinceMessage >= currentMessageDuration){
					HideAlert();
					showingAlert = false;
					if(popUpQueue.Count > 0){
						PopUpEvent popUp = popUpQueue[0];
						popUpQueue.RemoveAt(0);
						switch(popUp.speaker){
						case NPC.FIRECHIEF:
							ShowFireChief(popUp.message, popUp.duration);
							break;

						case NPC.MAYOR:
							ShowMayor(popUp.message, popUp.duration);
							break;

						case NPC.POLICECHIEF:
							ShowPoliceChief(popUp.message, popUp.duration);
							break;
						}
					}
				}
			}
		}
	}
}

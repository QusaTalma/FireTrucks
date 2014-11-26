using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopUpUIManager : MonoBehaviour {
	public GameObject fireChiefImage;
	public GameObject policeChiefImage;
	public GameObject mayorImage;
	public GameObject popUpPanel;
	public Text alertMessage;

	private List<PopUpEvent> popUpQueue = new List<PopUpEvent>();
	private bool showingAlert = false;

	public const float MESSAGE_DISPLAY_TIME = 5f;
	private float timeSinceMessage = 0f;
	
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

	public void ShowMayor(string message){
		if (!showingAlert) {
			showingAlert = true;
			timeSinceMessage = 0f;
			mayorImage.SetActive (true);
			alertMessage.text = message;
			popUpPanel.SetActive (true);
		} else {
			popUpQueue.Add(new PopUpEvent(message, NPC.MAYOR));
		}
	}

	public void ShowFireChief(string message){
		if (!showingAlert) {
			showingAlert = true;
			timeSinceMessage = 0f;
			fireChiefImage.SetActive (true);
			alertMessage.text = message;
			popUpPanel.SetActive (true);
		} else {
			popUpQueue.Add(new PopUpEvent(message, NPC.FIRECHIEF));
		}
	}

	public void ShowPoliceChief(string message){
		if (!showingAlert) {
			showingAlert = true;
			timeSinceMessage = 0f;
			policeChiefImage.SetActive (true);
			alertMessage.text = message;
			popUpPanel.SetActive (true);
		} else {
			popUpQueue.Add(new PopUpEvent(message, NPC.POLICECHIEF));
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

	void OnGUI(){
		if (timeSinceMessage <= MESSAGE_DISPLAY_TIME) {
			timeSinceMessage += Time.deltaTime;
			if(timeSinceMessage >= MESSAGE_DISPLAY_TIME){
				HideAlert();
				showingAlert = false;
				if(popUpQueue.Count > 0){
					PopUpEvent popUp = popUpQueue[0];
					popUpQueue.RemoveAt(0);
					switch(popUp.speaker){
					case NPC.FIRECHIEF:
						ShowFireChief(popUp.message);
						break;

					case NPC.MAYOR:
						ShowMayor(popUp.message);
						break;

					case NPC.POLICECHIEF:
						ShowPoliceChief(popUp.message);
						break;
					}
				}
			}
		}
	}
}

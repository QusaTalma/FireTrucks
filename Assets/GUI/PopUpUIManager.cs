using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

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

	public void ToggleRadio(){
		RadioOn = !RadioOn;
	}

	public void ShowMayor(string message, bool force=false){
		if ((!showingAlert || force) && radioOn) {
			HideAlert();
			showingAlert = true;
			timeSinceMessage = 0f;
			mayorImage.SetActive (true);
			alertMessage.text = message;
			popUpPanel.SetActive (true);
		} else {
			popUpQueue.Add(new PopUpEvent(message, NPC.MAYOR));
		}
	}

	public void ShowFireChief(string message, bool force=false){
		if ((!showingAlert || force) && radioOn) {
			HideAlert();
			showingAlert = true;
			timeSinceMessage = 0f;
			fireChiefImage.SetActive (true);
			alertMessage.text = message;
			popUpPanel.SetActive (true);
		} else {
			popUpQueue.Add(new PopUpEvent(message, NPC.FIRECHIEF));
		}
	}

	public void ShowPoliceChief(string message, bool force=false){
		if ((!showingAlert || force) && radioOn) {
			HideAlert();
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

	public void ShowFlameIndicator(EGFlame flame){
		if (radioOn) {			
			GameObject indicatorObject = (GameObject)Instantiate (flameIndicatorPrefab);
			UIFlameIndicator flameIndicator = indicatorObject.GetComponent<UIFlameIndicator> ();
			if (flameIndicator != null) {
				flameIndicator.Flame = flame;
			}
		}
	}

	void OnGUI(){
		if (timeSinceMessage <= MESSAGE_DISPLAY_TIME && radioOn) {
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

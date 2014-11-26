using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopUpUIManager : MonoBehaviour {
	public GameObject fireChiefImage;
	public GameObject policeChiefImage;
	public GameObject mayorImage;
	public GameObject popUpPanel;
	public Text alertMessage;

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
		HideAlert ();
		timeSinceMessage = 0f;
		mayorImage.SetActive (true);
		alertMessage.text = message;
		popUpPanel.SetActive (true);
	}

	public void ShowFireChief(string message){
		HideAlert ();
		timeSinceMessage = 0f;
		fireChiefImage.SetActive (true);
		alertMessage.text = message;
		popUpPanel.SetActive (true);
	}

	public void ShowPoliceChief(string message){
		HideAlert ();
		timeSinceMessage = 0f;
		policeChiefImage.SetActive (true);
		alertMessage.text = message;
		popUpPanel.SetActive (true);
	}

	public void HideAlert(){
		mayorImage.SetActive (false);
		policeChiefImage.SetActive (false);
		fireChiefImage.SetActive (false);
		popUpPanel.SetActive (false);
	}

	void OnGUI(){
		if (timeSinceMessage <= MESSAGE_DISPLAY_TIME) {
			timeSinceMessage += Time.deltaTime;
			if(timeSinceMessage >= MESSAGE_DISPLAY_TIME){
				HideAlert();
			}
		}
	}
}

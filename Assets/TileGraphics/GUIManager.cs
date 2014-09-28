using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public GUIStyle labelStyle;

	EGDispatcher _dispatcher;
	TGMap _map;

	const float LABEL_HEIGHT = 30;
	const float LABEL_WIDTH = 250;
	const float TRUCK_COUNT_TOP = 20;
	const float CITY_HEALTH_TOP = TRUCK_COUNT_TOP + LABEL_HEIGHT + 1;

	void Start(){
		_dispatcher = gameObject.GetComponent<EGDispatcher> ();
		_map = gameObject.GetComponent<TGMap> ();
	}

	void OnGUI(){
		DrawTruckCount ();
		DrawCityHealth ();
	}

	void DrawTruckCount(){
		float left, top, width, height;
		top = TRUCK_COUNT_TOP;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		left = Screen.width - width;
		string truckCount = _dispatcher.GetTruckCount () + " Firetrucks in play";
		GUI.Label (new Rect (left, top, width, height), truckCount, labelStyle); 
	}

	void DrawCityHealth(){
		float left, top, width, height;
		top = CITY_HEALTH_TOP;
		height = LABEL_HEIGHT;
		width = LABEL_WIDTH;
		left = Screen.width - width;
		float durabilityPercent = _map.GetCityDurabilityPercent ();
		string cityPercent = "Buildings remaining: " + durabilityPercent.ToString("P0");
		GUI.Label (new Rect (left, top, width, height), cityPercent, labelStyle); 
	}
}
using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public GUIStyle labelStyle;

	private TGMap _map;

	// Use this for initialization
	void Start () {
		_map = (TGMap)gameObject.GetComponent<TGMap> ();
	}

	void OnGUI(){
		float left, top, width, height;
		top = 20;
		height = 30;
		width = 300;
		left = Screen.width - width;
		string truckCount = _map.GetTruckCount () + " Firetrucks in play";
		GUI.Label (new Rect (left, top, width, height), truckCount, labelStyle); 
	}
}
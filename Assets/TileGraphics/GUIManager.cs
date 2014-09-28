﻿using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	public GUIStyle labelStyle;

	EGDispatcher _dispatcher;

	void Start(){
		_dispatcher = gameObject.GetComponent<EGDispatcher> ();
	}

	void OnGUI(){
		float left, top, width, height;
		top = 20;
		height = 30;
		width = 300;
		left = Screen.width - width;
		string truckCount = _dispatcher.GetTruckCount () + " Firetrucks in play";
		GUI.Label (new Rect (left, top, width, height), truckCount, labelStyle); 
	}
}
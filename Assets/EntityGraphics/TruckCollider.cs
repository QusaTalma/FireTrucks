using UnityEngine;
using System.Collections;

public class TruckCollider : MonoBehaviour {

	public GameObject truckObject;

	EGFiretruck truck;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other){
		Debug.Log ("Triggered!");
	}

	void OnTriggerExit(Collider other){
		Debug.Log ("Untriggered!");
	}
}
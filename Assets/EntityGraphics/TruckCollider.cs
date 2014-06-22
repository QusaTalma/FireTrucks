using UnityEngine;
using System.Collections;

public class TruckCollider : MonoBehaviour {

	public GameObject truckObject;

	EGFiretruck truck;

	void Start(){
		truck = truckObject.GetComponent<EGFiretruck> ();
	}
	
	void OnTriggerEnter(Collider other){
		if(!transform.parent.Equals(other.transform.parent) &&
		   other.gameObject.tag.Equals("Truck")){
			truck.SetWaitingForTraffic(true);
		}
	}

	void OnTriggerExit(Collider other){
		if(!transform.parent.Equals(other.transform.parent) &&
		   other.gameObject.tag.Equals("Truck")){
			truck.SetWaitingForTraffic(false);
		}
	}
}
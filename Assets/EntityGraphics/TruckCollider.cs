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
			EGFiretruck otherTruck = other.transform.root.gameObject.GetComponent<EGFiretruck>();
			if(otherTruck != null){
				truck.addOtherTruck(other.transform.root.gameObject);
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(!transform.parent.Equals(other.transform.parent) &&
		   other.gameObject.tag.Equals("Truck")){
			EGFiretruck otherTruck = other.transform.root.gameObject.GetComponent<EGFiretruck>();
			if(otherTruck != null){
				truck.removeOtherTruck(other.transform.root.gameObject);
			}
		}
	}
}
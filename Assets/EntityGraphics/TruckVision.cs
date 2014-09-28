using UnityEngine;
using System.Collections;

public class TruckVision : MonoBehaviour {
	public EGFiretruck truck;
	public EGHose hose;

	void OnTriggerEnter(Collider other){
		if(!transform.parent.Equals(other.transform.parent) &&
		   other.gameObject.tag.Equals("Fire")){
			hose.AddFlame(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other){
		if(!transform.parent.Equals(other.transform.parent) &&
		   other.gameObject.tag.Equals("Fire")){
			hose.RemoveFlame(other.gameObject);
		}
	}
}
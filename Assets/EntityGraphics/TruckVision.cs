using UnityEngine;
using System.Collections;

public class TruckVision : MonoBehaviour {
	public EGFiretruck truck;
	public EGHose hose;

	void OnTriggerEnter(Collider other){
		if(!transform.root.Equals(other.transform.root)){
			string otherTag = other.gameObject.tag;
			if(otherTag.Equals("Fire")){
				hose.AddFlame(other.gameObject);
			//}else if(otherTag.Equals("Truck")){
			//	truck.addOtherTruck(other.gameObject);
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if(!transform.root.Equals(other.transform.root)){
			string otherTag = other.gameObject.tag;
			if(otherTag.Equals("Fire")){
				hose.RemoveFlame(other.gameObject);
			//}else if(otherTag.Equals("Truck")){
			//	truck.removeOtherTruck(other.gameObject);
			}
		}
	}
}
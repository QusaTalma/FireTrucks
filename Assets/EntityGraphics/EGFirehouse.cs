using UnityEngine;
using System.Collections;

public class EGFirehouse : MonoBehaviour {
	int truckCount = 0;

	public bool ContainsTruck(){
		return truckCount > 0;
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.tag.Equals("Truck")){
			truckCount++;
		}
	}

	void OnTriggerExit(Collider other){
		Debug.Log ("Trigger exit! " + other.gameObject.tag);
		if(other.gameObject.tag.Equals("Truck")){
			truckCount--;
		}
	}
}
using UnityEngine;
using System.Collections;

public class EGFirehouse : MonoBehaviour {
	int truckCount = 0;

	public bool ContainsTruck(){
		return truckCount > 0;
	}

	public void AddTruck(){
		truckCount++;
	}

//	void OnTriggerEnter(Collider other){
//		if(other.gameObject.tag.Equals("Truck")){
//			truckCount++;
//		}
//	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag.Equals("Truck")){
			truckCount--;
		}
	}
}
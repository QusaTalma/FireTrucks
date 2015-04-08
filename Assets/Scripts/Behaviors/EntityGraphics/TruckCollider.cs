using UnityEngine;
using System.Collections;

namespace Behaviors.EntityGraphics{
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
				Vector3 closestPoint = other.ClosestPointOnBounds(truck.GetPosition());
				if(otherTruck != null){
					truck.addOtherTruck(other.transform.root.gameObject, closestPoint);
				}
			}
		}

		void OnTriggerStay(Collider other){
			if(!transform.parent.Equals(other.transform.parent) &&
			   other.gameObject.tag.Equals("Truck")){
				EGFiretruck otherTruck = other.transform.root.gameObject.GetComponent<EGFiretruck>();
				Vector3 closestPoint = other.ClosestPointOnBounds(truck.GetPosition());
				if(otherTruck != null){
					truck.addOtherTruck(other.transform.root.gameObject, closestPoint);
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
}
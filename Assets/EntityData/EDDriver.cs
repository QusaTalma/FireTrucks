using System;
using UnityEngine;

public class EDDriver {
	private EDTruckControls controls;
	private Vector3 steering;

	private bool queueing = false;

	public EDDriver (EDTruckControls controls) {
		this.controls = controls;
		steering = new Vector3 ();
	}

	public void Seek(Vector3 target){
		steering = steering + doSeek (target);
	}

	private Vector3 doSeek(Vector3 target){
		Vector3 force;
		Vector3 desiredVelocity = Vector3.Normalize(target - controls.GetPosition());
		desiredVelocity = desiredVelocity * controls.GetMaxVelocity();
		force = desiredVelocity - controls.GetVelocity();

		return force;
	}

	public void Avoid(GameObject target){
		steering = steering + doAvoid (target);
	}

	private Vector3 doAvoid(GameObject target){
		Vector3 velocity = controls.GetVelocity();
		RaycastHit hitInfo;
		Vector3 avoidPos = new Vector3();
		if(Physics.Raycast(controls.GetPosition(), velocity, out hitInfo, controls.GetMaxAvoidance())){
			if(hitInfo.transform.gameObject.tag.Equals("Truck")){
				EGFiretruck otherTruck = hitInfo.transform.root.GetComponent<EGFiretruck>();
				if(otherTruck != null && 
				   otherTruck.transform.position != controls.GetPosition()){
					Debug.Log("Avoiding a truck");
					avoidPos = hitInfo.point;
				}
			}
		}

		Vector3 avoidance = new Vector3 ();

		if(avoidPos != new Vector3()){
			avoidance = Vector3.Normalize(avoidPos - controls.GetPosition());
			
			Debug.Log ("Avoidance force: " + avoidance);
			Debug.Log ("Target: " + avoidPos);
			Debug.Log ("Position: " + controls.GetPosition ());
		}


		return avoidance;
	}

	public bool Queue(GameObject target){
		Vector3 queueVector = doQueue(target);
		steering = steering + queueVector;
		return queueing;
	}

	private Vector3 doQueue(GameObject target){
		Vector3 queueForce = new Vector3 ();
//
//		Vector3 ahead = controls.GetVelocity();
//		ahead.Normalize();
//		ahead = ahead * controls.GetMaxQueueAhead();
//
//		ahead = controls.GetPosition() + ahead;
//
//		Vector3 nearestPoint = target.transform.position;
//		float dist = Vector3.Distance(ahead, nearestPoint);
//
//		float tolerance = controls.GetMaxQueueRadius();// + target.transform.localScale.z;

		//Vector3 toNearest = nearestPoint - controls.GetPosition();
		//float angleToNearest = Vector3.Angle(ahead, toNearest);
		//if(angleToNearest > 90){
//			if(dist <= tolerance){
				queueing = true;
//			}
		//}

		return queueForce;
	}

	public bool IsQueueing(){
		return queueing;
	}

	public void Update(float delta){
		float maxVelocity = delta * controls.GetMaxVelocity ();
		Vector3 velocity = controls.GetVelocity ();
		
		steering = VectorUtils.Truncate (steering, maxVelocity);
		
		velocity = velocity + steering;
		velocity = VectorUtils.Truncate (velocity, maxVelocity);
		controls.SetVelocity (velocity);
	}

	public void Reset(){
		steering = new Vector3 ();
		queueing = false;
	}
}
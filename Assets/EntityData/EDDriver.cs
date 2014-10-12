using System;
using UnityEngine;

public class EDDriver {
	private EDTruckControls controls;
	private Vector3 steering;

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

	public void Avoid(Vector3 target){
		steering = steering + doAvoid (target);
	}

	private Vector3 doAvoid(Vector3 target){
		Vector3 avoidance = new Vector3 ();

		avoidance = Vector3.Normalize(target - controls.GetPosition());
		avoidance = avoidance * controls.GetMaxAvoidance ();

		Debug.Log ("Avoidance force: " + avoidance);
		Debug.Log ("Target: " + target);
		Debug.Log ("Position: " + controls.GetPosition ());

		return avoidance;
	}

	public void Queue(Vector3 target){
		steering = steering + doQueue (target);
	}

	private Vector3 doQueue(Vector3 target){
		Vector3 queueForce = new Vector3 ();


		return queueForce;
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
	}

}
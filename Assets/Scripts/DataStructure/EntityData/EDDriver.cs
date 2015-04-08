using System;
using UnityEngine;
using Utils;

namespace DataStructure.EntityData{
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

			Debug.DrawLine (controls.GetPosition (), controls.GetPosition () + force, Color.green);

			return force;
		}

		public bool Queue(GameObject target){
			Vector3 queueVector = doQueue(target);

			steering = steering + queueVector;
			return queueing;
		}

		private Vector3 doQueue(GameObject target){
			Vector3 queueForce = new Vector3 ();
			queueing = true;

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
}
using System;
using UnityEngine;

namespace DataStructure.EntityData{
	public interface EDTruckControls{
		Vector3 GetVelocity();
		float GetMaxVelocity();
		Vector3 GetPosition();
		void SetVelocity(Vector3 velocity);
		float GetMaxAvoidance();
		Vector3 GetLeft();
	}
}
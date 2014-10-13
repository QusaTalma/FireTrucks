using System;
using UnityEngine;
public interface EDTruckControls{
	Vector3 GetVelocity();
	float GetMaxVelocity();
	Vector3 GetPosition();
	void SetVelocity(Vector3 velocity);
	float GetMaxAvoidance();
	float GetAvoidanceForce();
}
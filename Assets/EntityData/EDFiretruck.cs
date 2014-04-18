using UnityEngine;

public class EDFiretruck{
	Vector3 position;
	Vector3 destination;
	float speed;

	public float GetSpeed(){
		return speed;
	}

	public void SetSpeed(float speed){
		this.speed = speed;
	}

	public void SetPosition(Vector3 position){
		this.position = position;
	}

	public Vector3 GetPosition(){
		return position;
	}

	public void SetDestination(Vector3 destination){
		this.destination = destination;
	}

	public Vector3 GetDestination(){
		return destination;
	}

	public void Update(float deltaTime){
		position = Vector3.Lerp (position, destination, speed * deltaTime);
	}
}
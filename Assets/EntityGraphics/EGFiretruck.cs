using UnityEngine;
using System.Collections;

public class EGFiretruck : MonoBehaviour {
	EDFiretruck _firetruck = new EDFiretruck();
	TGMap map;
	Vector3 destination;

	bool waitingForTraffic = false;
	bool puttingOutFire = false;
	bool idle = false;

	Vector3 velocity = Vector3.forward;

	public float speed;
	public float maxSteering;
	public bool returnWhenIdle;
	public float destinationRadius;

	void Update(){
		//Update distance and angle to destination
		float dist = Vector3.Distance (transform.position, destination);
		//Check if the truck is at its destination or a destination
		//is needed
		bool destinationReached = dist <= destinationRadius;
		bool destinationNotSet = destination.Equals (new Vector3 ());

		if (destinationReached || destinationNotSet) {
			SetDestination(GetNextDestination());

			if(destination.Equals(new Vector3())){
				return;
			}
		}

		if (waitingForTraffic || puttingOutFire) {
			return;
		}

		dist = Vector3.Distance (transform.position, destination);
		float deltaTime = Time.deltaTime;
		float maxMagnitude = speed * deltaTime;
		maxMagnitude = Mathf.Min (maxMagnitude, dist);

		UpdateVelocity (maxMagnitude);

		transform.forward = velocity;
		transform.Translate(Vector3.forward * maxMagnitude);
	}

	bool HasNextDestination(){
		return _firetruck.HasNextStep ();
	}

	Vector3 GetNextDestination(){
		TDStep step = _firetruck.PopPathStep ();
		TDStep afterStep = _firetruck.PeekPathStep ();
		Vector3 nextPosition = new Vector3();
		Vector3 afterPosition = new Vector3 ();
		if (step != null) {
			TDTile nextTile = step.tile;
			TDTile afterTile = null;

			if(afterStep != null){
				afterTile = afterStep.tile;
				afterPosition = map.GetPositionForTile(afterTile.GetX(), afterTile.GetY());
				afterPosition.x += 0.5f;
				afterPosition.z -= 0.5f;
			}

			nextPosition = map.GetPositionForTile (nextTile.GetX (), nextTile.GetY ());

			nextPosition.x += 0.5f;
			nextPosition.z -= 0.5f;

			//If nextPosition is to the right then trucks drive in the lower
			//of the horizontal lanes
			if(nextPosition.x > transform.position.x 
			   		|| (afterStep != null && afterPosition.x > transform.position.x)){
				nextPosition.z -= 0.25f;
			//If it's to the left then trucks drive in the upper lane
			}else if(nextPosition.x < transform.position.x 
			         || (afterStep != null && afterPosition.x < transform.position.x)){
				nextPosition.z += 0.25f;
			}

			//If nextPosition is up the trucks drive in the right lane
			if(nextPosition.z > transform.position.z 
			   		|| (afterStep != null && afterPosition.z > transform.position.z)){
				nextPosition.x += 0.25f;
			//If it's down then drucks drive in the left lane
			}else if(nextPosition.z < transform.position.z
			         || (afterStep != null && afterPosition.z < transform.position.z)){
				nextPosition.x -= 0.25f;
			}
		}

		return nextPosition;
	}

	void UpdateVelocity(float maxMagnitude){
		Vector3 desiredVelocity = Vector3.Normalize(destination - transform.position);
		desiredVelocity = desiredVelocity * maxMagnitude;
		
		Vector3 steering = VectorUtils.Truncate( desiredVelocity - velocity, maxSteering);
		
		velocity = velocity + steering;
		velocity = VectorUtils.Truncate (velocity, maxMagnitude);
	}

	public void SetWaitingForTraffic(bool waiting){
		this.waitingForTraffic = waiting;
	}

	public bool IsWaitingForTraffic(){
		return waitingForTraffic;
	}

	public void SetMap(TGMap map){
		this.map = map;
	}

	public void SetPath(TDPath path){
		_firetruck.SetPath (path);

		puttingOutFire = false;
		waitingForTraffic = false;
		SetDestination (GetNextDestination ());
	}

	public Vector3 GetPosition(){
		return transform.position;
	}

	public void SetPosition(Vector3 position){
		transform.position = position;
	}

	public void SetDestination(Vector3 destination){
		this.destination = destination;
	}

	public bool IsActive(){
		return (!destination.Equals(new Vector3()) || HasNextDestination()) && !idle;
	}

	public bool IsPuttingOutFire(){
		return puttingOutFire;
	}

	public void SetPuttingOutFire(bool puttingOutFire){
		this.puttingOutFire = puttingOutFire;
	}

	public void SetIdle(bool idle){
		this.idle = idle;
	}
}
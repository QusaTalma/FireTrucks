using UnityEngine;
using System.Collections;

public class EGFiretruck : MonoBehaviour {
	EDFiretruck _firetruck = new EDFiretruck();
	TGMap map;
	Vector3 position;
	Vector3 destination;
	float angleToDestination = 0;

	bool waitingForTraffic = false;
	bool puttingOutFire = false;

	public float speed;
	public float turnSpeed;

	void Update(){
		//Check if the truck is at its destination or a destination
		//is needed
		bool destinationReachedOrNotSet = destination.Equals(new Vector3()) ||
			destination.Equals(transform.position);
		if (destinationReachedOrNotSet) {
			SetDestination(GetNextDestination());

			angleToDestination = CalculateAngleToDestination();

			if(destination.Equals(new Vector3())){
				return;
			}
		}

		if (waitingForTraffic || puttingOutFire) {
			return;
		}

		float deltaTime = Time.deltaTime;

		//Update distance and angle to destination
		float dist = Vector3.Distance (position, destination);
		angleToDestination = CalculateAngleToDestination();

		//Ugh, this sucks, but to prevent a stupid fucking issue where
		//the truck turns back and forth across the destination angle 
		//when the difference is very small, usually aboout 6.0E-5, so if the
		//decimal is in the E-5 magnitude ignore it
		if (dist > 0 && Mathf.Abs(angleToDestination) > 0.0001){
			float rotateBy;
			if(Mathf.Abs(angleToDestination) > Mathf.Abs(turnSpeed*deltaTime)){
				rotateBy = turnSpeed * deltaTime;
				if(angleToDestination < 0){
					rotateBy = 0 - rotateBy;
				}
			}else{
				rotateBy = angleToDestination;
			}

			transform.Rotate(0, rotateBy, 0);
		}else if (dist > speed*deltaTime){
			transform.Translate(Vector3.forward * speed * deltaTime);
		}else{
			transform.position = destination;
		}

		position = transform.position;
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

	float CalculateAngleToDestination(){
		//This angle is in a coordinate space of (-180,180] with 0 to the right and increasing to the counterclockwise
		float angle = Mathf.Atan2 (destination.z - transform.position.z, destination.x - transform.position.x);
		angle = angle * Mathf.Rad2Deg;

		//Convert to local coordinate system
		angle = 90 - angle;

		float localizedCurrentAngle = transform.eulerAngles.y;
		angle = angle - localizedCurrentAngle;
		angle = (angle+360) % 360;

		if (angle > 180) {
			angle = angle - 360;
		}

		return angle;
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

		SetDestination (GetNextDestination ());
		angleToDestination = CalculateAngleToDestination();
	}
	
	public void SetPosition(Vector3 position){
		this.position = position;
	}

	public Vector3 GetPosition(){
		return this.position;
	}

	public void SetDestination(Vector3 destination){
		this.destination = destination;
	}

	public bool IsActive(){
		return !destination.Equals(new Vector3()) || HasNextDestination();
	}

	public bool IsPuttingOutFire(){
		return puttingOutFire;
	}

	public void SetPuttingOutFire(bool puttingOutFire){
		this.puttingOutFire = puttingOutFire;
	}
}
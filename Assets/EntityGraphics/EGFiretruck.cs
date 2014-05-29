using UnityEngine;
using System.Collections;

public class EGFiretruck : MonoBehaviour {
	EDFiretruck _firetruck = new EDFiretruck();
	TGMap map;
	Vector3 position;
	Vector3 destination;
	float angleToDestination = 0;

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

		float deltaTime = Time.deltaTime;

		//Update distance and angle to destination
		float dist = Vector3.Distance (position, destination);
		angleToDestination = CalculateAngleToDestination();

		if (dist > 0 && Mathf.Abs(angleToDestination) > 0){
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
		Vector3 nextPosition = new Vector3();
		if (step != null) {
			TDTile nextTile = step.tile;
			nextPosition = map.GetPositionForTile (nextTile.GetX (), nextTile.GetY ());

			nextPosition.x += 0.5f;
			nextPosition.z -= 0.5f;
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

//		if (angle < 0) {
//			angle = Mathf.Ceil (angle);
//		} else {
//			angle = Mathf.Floor(angle);
//		}

		return angle;
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
}
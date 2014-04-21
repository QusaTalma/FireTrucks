using UnityEngine;
using System.Collections;

public class EGFiretruck : MonoBehaviour {
	EDFiretruck _firetruck = new EDFiretruck();
	TGMap map;
	Vector3 position;
	Vector3 destination;
	public float speed;

	void Update(){
		if (destination == null) {
			SetDestination(GetNextDestination());
		}

		float deltaTime = Time.deltaTime;
		float xDiff = destination.x - transform.position.x;
		float zDiff = destination.z - transform.position.z;
		float angle = Mathf.Atan2 (zDiff, xDiff);
		float dist = Vector3.Distance (position, destination);

		if (dist > speed*deltaTime) {
			transform.Translate (Mathf.Cos (angle) * speed * deltaTime, 0, Mathf.Sin (angle) * speed * deltaTime);
		} else {
			transform.position = destination;
			destination = GetNextDestination();
		}

		position = transform.position;
	}

	Vector3 GetNextDestination(){
		TDStep step = _firetruck.PopPathStep ();
		Vector3 nextPosition = destination;
		if (step != null) {
			TDTile nextTile = step.tile;
			nextPosition = map.GetPositionForTile (nextTile.GetX (), nextTile.GetY ());

			nextPosition.x += 0.5f;
			nextPosition.z -= 0.5f;
		}

		return nextPosition;
	}

	public void SetMap(TGMap map){
		this.map = map;
	}

	public void SetPath(TDPath path){
		_firetruck.SetPath (path);

		SetDestination (GetNextDestination ());
	}
	
	public void SetPosition(Vector3 position){
		this.position = position;
	}

	public void SetDestination(Vector3 destination){
		this.destination = destination;
	}
}
using UnityEngine;
using System.Collections;

public class EGFiretruck : MonoBehaviour {
	EDFiretruck _firetruck;
	Vector3 position;
	Vector3 destination;
	public float speed;

	// Use this for initialization
	void Start () {
		_firetruck = new EDFiretruck ();
	}

	void Update(){
		float deltaTime = Time.deltaTime;
		float xDiff = destination.x - transform.position.x;
		float zDiff = destination.z - transform.position.z;
		float angle = Mathf.Atan2 (zDiff, xDiff);
		float dist = Vector3.Distance (position, destination);

		if (dist > speed*deltaTime) {
			transform.Translate (Mathf.Cos (angle) * speed * deltaTime, 0, Mathf.Sin (angle) * speed * deltaTime);
		} else {
			speed = 0;
			transform.position = destination;
		}

		position = transform.position;
	}
	
	public void SetPosition(Vector3 position){
		this.position = position;
	}

	public void SetDestination(Vector3 destination){
		this.destination = destination;
	}
}
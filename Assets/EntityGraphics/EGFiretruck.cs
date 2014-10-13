using UnityEngine;
using System.Collections.Generic;

public class EGFiretruck : MonoBehaviour, EDTruckControls{
	EDFiretruck _firetruck = new EDFiretruck();
	EDDriver _driver;
	TGMap map;
	Vector3 destination;

	List<GameObject> otherTrucks;

	bool puttingOutFire = false;
	bool idle = false;

	Vector3 velocity = Vector3.forward;

	public float speed;
	public bool returnWhenIdle;
	public float destinationRadius;
	public float avoidanceDistance;
	public float maxAvoidance;
	public float avoidanceForce;

	void Start(){
		_driver = new EDDriver (this);
		otherTrucks = new List<GameObject> ();
	}

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

		//If the truck is waiting on traffic then look beyond
		//the current tile for directional guidance
		if (_driver.IsQueueing ()) {
			IncrementDestinationIfOnCurrentDestinationTile();
		}

		if (puttingOutFire) {
			return;
		}

		dist = Vector3.Distance (transform.position, destination);
		float deltaTime = Time.deltaTime;
		float maxMagnitude = speed * deltaTime;
		maxMagnitude = Mathf.Min (maxMagnitude, dist);

		UpdateVelocity ();

		if(velocity.magnitude > 0){
			transform.forward = velocity;
		}

		if (!_driver.IsQueueing()) {
			transform.Translate(Vector3.forward * maxMagnitude);
		}else{
			transform.Translate(Vector3.zero);
		}
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
			//If it's down then trucks drive in the left lane
			}else if(nextPosition.z < transform.position.z
			         || (afterStep != null && afterPosition.z < transform.position.z)){
				nextPosition.x -= 0.25f;
			}
		}

		return nextPosition;
	}

	void UpdateVelocity(){
		_driver.Reset();
		_driver.Seek (destination);
		AvoidTrucks (otherTrucks);
		QueueBehindTrucks(otherTrucks);
		_driver.Update (Time.deltaTime);
	}

	void AvoidTrucks(List<GameObject> trucksToAvoid){
		if (trucksToAvoid != null){
			for(int i=0; i<trucksToAvoid.Count; i++){
				GameObject truckToAvoid = trucksToAvoid[i];
				if(truckToAvoid != null){
					_driver.Avoid(truckToAvoid);
				}
			}
		}
	}

	void QueueBehindTrucks(List<GameObject> nearbyTrucks){
		if(nearbyTrucks != null){
			for(int i=0; i<nearbyTrucks.Count; i++){
				GameObject truck = nearbyTrucks[i];
				if(_driver.Queue(truck)){
					//Stop looping if this truck is queueing behind another truck
					break;
				}
			}
		}
	}

	void IncrementDestinationIfOnCurrentDestinationTile(){
		TDTile currentTile = map.GetTileForWorldPosition (transform.position);
		TDTile destinationTile = map.GetTileForWorldPosition (destination);
		if (currentTile.Equals (destinationTile)) {
			SetDestination(GetNextDestination());
		}
	}

	public bool IsWaitingForTraffic(){
		return _driver.IsQueueing();
	}

	public void SetMap(TGMap map){
		this.map = map;
	}

	public void SetPath(TDPath path){
		_firetruck.SetPath (path);

		puttingOutFire = false;
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

	public Vector3 GetVelocity()
	{
		return velocity;
	}

	public float GetMaxVelocity()
	{
		return speed;
	}

	public void SetVelocity(Vector3 velocity){
		this.velocity = velocity;
	}

	public float GetMaxAvoidance(){
		return this.maxAvoidance;
	}

	public void addOtherTruck(GameObject other){
		if(other.transform.root != transform.root){
			otherTrucks.Add (other);
		}
	}

	public void removeOtherTruck(GameObject other){
		otherTrucks.Remove (other);
	}

	public float GetAvoidanceForce(){
		return avoidanceForce;
	}
}
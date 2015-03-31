using UnityEngine;
using System.Collections.Generic;

public class EGFiretruck : MonoBehaviour, EDTruckControls{
	EDFiretruck _firetruck = new EDFiretruck();
	EDDriver _driver;
	TGMap map;
	Vector3 destination;

	private GameObject targetFlame = null;
	public GameObject TargetFlame{
		set { this.targetFlame = value; }
		get { return this.targetFlame; }
	}

	Dictionary<GameObject, Vector3> otherTrucks;

	bool puttingOutFire = false;
	bool idle = false;
	bool selected = false;

	Vector3 velocity = Vector3.forward;

	private const float FIRE_BUFFER = 0.3f;//It's the z scale of the body of the truck

	public float speed;
	public bool returnWhenIdle;
	public float destinationRadius;
	public float maxAvoidance;

	void Start(){
		_driver = new EDDriver (this);
		otherTrucks = new Dictionary<GameObject, Vector3> ();
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
				//If the truck is currently putting out a fire then it should drive as close to the fire as possible
				if(puttingOutFire){
					ApproachTargetFlame();
				}else{
					return;
				}
			}
		}

		//If the truck is waiting on traffic then look beyond
		//the current tile for directional guidance
		if (_driver.IsQueueing ()) {
			IncrementDestinationIfOnCurrentDestinationTile();
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
		QueueBehindTrucks();
		_driver.Update (Time.deltaTime);
	}

	void QueueBehindTrucks(){
		if(otherTrucks != null){Dictionary<GameObject, Vector3>.Enumerator enumerator = otherTrucks.GetEnumerator();
			while(enumerator.MoveNext()){
				GameObject truck = enumerator.Current.Key;
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

	private void ApproachTargetFlame(){
		if (targetFlame != null) {
			TDTile currentTile = map.GetTileForWorldPosition (transform.position);
			TDTile flameTile = map.GetTileForWorldPosition(targetFlame.transform.position);

			if(currentTile.IsOtherAdjacent(flameTile)){
				destination = currentTile.FindPointAdjacentToTileWithBuffer(flameTile, FIRE_BUFFER, transform.position);
			}else{
				EGDispatcher.Instance.SendTruckToTile(this, flameTile.GetX(), flameTile.GetY());
			}
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

	public void addOtherTruck(GameObject other, Vector3 closestPoint){
		if(other.transform.root != transform.root){
			if(otherTrucks.ContainsKey(other)){
				otherTrucks.Remove(other);
			}
			otherTrucks.Add (other, closestPoint);
		}
	}

	public void removeOtherTruck(GameObject other){
		otherTrucks.Remove (other);
	}

	public Vector3 GetLeft(){
		return -transform.right;
	}

	public void SetSelected(bool newSelected){
		selected = newSelected;
	}

	public bool IsSelected(){
		return selected;
	}
}
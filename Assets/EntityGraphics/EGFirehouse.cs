using UnityEngine;
using System.Collections;

public class EGFirehouse : MonoBehaviour {
	int truckCount = 0;

	private EDFirehouse _firehouse;
	private EDDispatcher _dispatcher;
	TGMap _map;

	public GameObject firetruckPrefab;


	void Start(){
		_firehouse = new EDFirehouse();
	}

	void Update(){//Spawn truck is ready for it
		if (!ContainsTruck ()) {
			SpawnNextTruck ();
		}
	}

	public void SetTGMap(TGMap map){
		_map = map;
	}

	public void SetDispatcher(EDDispatcher dispatcher){
		_dispatcher = dispatcher;
	}

	public bool ContainsTruck(){
		return truckCount > 0;
	}

	public void AddTruck(){
		truckCount++;
	}

	void OnTriggerEnter(Collider other){
//		if(other.gameObject.tag.Equals("Truck")){
//			truckCount++;
//		}
	}

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag.Equals("Truck")){
			truckCount--;
		}
	}

	public void DecreaseTruckCount(){
		if(truckCount > 0){
			truckCount--;
		}
	}
	
	public void AddPositionToSpawnQueue(Vector2 truckPosition){
		_firehouse.AddPositionToSpawnQueue(truckPosition);
	}
	
	public void SpawnNextTruck(){
		if (_firehouse.GetSpawnQueue().Count > 0) {
			Vector2 truckPos = _firehouse.PopSpawnQueue();
			SpawnFireTruck((int)truckPos.x, (int)truckPos.y);
		}
	}
	
	//Creates a firetruck at the firehouse with
	//the tile at the given point as destination
	public void SpawnFireTruck(int x, int z){
		Vector2 fireHouseTilePos;
		_map.GetDataMap().GetFireHouseCoordinates (out fireHouseTilePos);
		
		Vector3 truckPos = _map.GetPositionForTile (Mathf.FloorToInt(fireHouseTilePos.x),
		                                       Mathf.FloorToInt(fireHouseTilePos.y));
		truckPos.x += .5f;
		truckPos.z -= .5f;
		
		AddTruck ();
		
		GameObject truck = (GameObject)Instantiate (firetruckPrefab);
		truck.transform.position = truckPos;
		
		EGFiretruck firetruck = truck.GetComponent<EGFiretruck>();
		firetruck.SetPosition (truckPos);
		
		TDPath truckPath = new TDPath ();
		TDMap dataMap = _map.GetDataMap();
		firetruck.SetMap (_map);
		truckPath.BuildPath (dataMap,
		                     dataMap.GetTile(Mathf.FloorToInt(fireHouseTilePos.x), Mathf.FloorToInt(fireHouseTilePos.y)),
		                     dataMap.GetTile (x, -z));
		
		firetruck.SetPath (truckPath);
		_dispatcher.AddActiveTruck(firetruck);
	}
}
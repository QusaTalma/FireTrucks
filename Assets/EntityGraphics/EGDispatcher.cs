using UnityEngine;
using System.Collections.Generic;

public class EGDispatcher : MonoBehaviour {
	private EDDispatcher _dispatcher;

	TGMap _map;
	EGFirehouse _firehouse;

	EGFiretruck selectedTruck = null;

	// Use this for initialization
	void Start () {
		_map = GetComponent<TGMap>();
		_dispatcher = new EDDispatcher();
		if (_firehouse != null) {
			_firehouse.SetDispatcher(_dispatcher);
		}
	}

	public void SetFirehouse(EGFirehouse firehouse){
		_firehouse = firehouse;
		_firehouse.SetDispatcher(_dispatcher);
	}
	
	// Update is called once per frame
	void Update () {
		//Sort through trucks so that they are kept in the right sets, active and idle
		List<EGFiretruck> toMoveToIdle = new List<EGFiretruck> ();
		for (int i=0; i<_dispatcher.GetActiveTrucks().Count; i++) {
			EGFiretruck truck = _dispatcher.GetActiveTruckAtIndex(i);
			
			if(!truck.IsActive() && !truck.IsPuttingOutFire()){
				//Don't remove them while iterating, remove afterwards
				toMoveToIdle.Add(truck);
			}
		}
		
		//Set the trucks that need setting
		for (int i=0; i<toMoveToIdle.Count; i++) {
			EGFiretruck truck = toMoveToIdle[i];
			SetTruckIdle(truck);
		}
		
		//Identify trucks that need to be removed
		List<EGFiretruck> toDestroy = new List<EGFiretruck> ();
		
		for (int i=0; i<_dispatcher.GetIdleTrucks().Count; i++) {
			EGFiretruck truck = _dispatcher.GetIdleTruckAtIndex(i);
			
			TDTile truckTile = _map.GetTileForWorldPosition(truck.transform.position);
			//Trucks that are idle at the firehouse need to be removed
			if(truckTile.type == TDTile.TILE_FIREHOUSE){
				toDestroy.Add(truck);
			}
		}
		
		//Remove trucks that need to be removed
		for (int i=0; i<toDestroy.Count; i++) {
			EGFiretruck truck = toDestroy[i];
			_dispatcher.RemoveIdleTruck(truck);
			_firehouse.DecreaseTruckCount();
			Destroy(truck.gameObject);
		}
	}

	public void SetTruckIdle(EGFiretruck truck){
		if(truck != null){
			_dispatcher.RemoveActiveTruck(truck);
			_dispatcher.AddIdleTruck(truck);
			
			if(truck.returnWhenIdle){
				Vector2 fireHouseTilePos = new Vector2 ();
				_map.GetDataMap().GetFireHouseCoordinates (out fireHouseTilePos);
				
				TDTile start = _map.GetTileForWorldPosition(truck.transform.position);
				TDTile end = _map.GetDataMap().GetTile(Mathf.FloorToInt(fireHouseTilePos.x), Mathf.FloorToInt(fireHouseTilePos.y));
				TDPath pathToFirehouse = new TDPath();
				pathToFirehouse.BuildPath (_map.GetDataMap(), start, end);
				
				truck.SetPath(pathToFirehouse);
			}
			
			truck.SetIdle(true);
		}
	}

	public void SetSelectedTruck(EGFiretruck selected){
		selectedTruck = selected;
	}
	
	public void AddPositionToSpawnQueue(Vector2 truckPosition){
		_firehouse.AddPositionToSpawnQueue(truckPosition);
	}

	public int GetTruckCount(){
		return _dispatcher.GetTruckCount();
	}

	public void SendIdleToPosition(int x, int z){
		EGFiretruck truckToSend = null;
		
		if (selectedTruck != null) {
			truckToSend = selectedTruck;
			_dispatcher.RemoveIdleTruck (truckToSend);
			selectedTruck = null;
		} else if (_dispatcher.GetIdleTrucks().Count > 0) {
			truckToSend = _dispatcher.PopIdleTruck ();
		}
		
		if(truckToSend != null){
			TDPath truckPath = new TDPath ();
			TDMap dataMap = _map.GetDataMap();
			truckPath.BuildPath (dataMap,
			                     dataMap.GetTile(Mathf.FloorToInt(truckToSend.GetPosition().x), Mathf.FloorToInt(-truckToSend.GetPosition().z)),
			                     dataMap.GetTile (x, -z));
			truckToSend.SetPath(truckPath);
			truckToSend.SetIdle(false);

			_dispatcher.AddActiveTruck(truckToSend);
		}
	}
}

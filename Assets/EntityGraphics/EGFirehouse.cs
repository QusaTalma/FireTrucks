using UnityEngine;
using System.Collections;

public class EGFirehouse : MonoBehaviour {
	int truckCount = 0;

	private EDFirehouse _firehouse;
	private EDDispatcher _dispatcher;
	TGMap _map;

	public GameObject firetruckPrefab;

	//the current frame to display
	private static int lastIndex = 0;
	
	void Start()
	{
		_firehouse = new EDFirehouse();

		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / UnifiedAnimator.FLAME_COLUMNS, 1f / UnifiedAnimator.FLAME_ROWS);
		renderer.sharedMaterial.SetTextureScale("_MainTex", size);
	}
	
	void Update(){
		if (lastIndex != UnifiedAnimator.FlameFrame) {
			lastIndex = UnifiedAnimator.FlameFrame;
			//split into x and y indexes
			Vector2 offset = new Vector2((float)lastIndex / UnifiedAnimator.FLAME_COLUMNS - (lastIndex / UnifiedAnimator.FLAME_COLUMNS), //x index
			                             (lastIndex / UnifiedAnimator.FLAME_COLUMNS) / (float)UnifiedAnimator.FLAME_ROWS);          //y index
			
			renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
		}
		
		if (truckCount == 0) {
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

	void OnTriggerExit(Collider other){
		if(other.gameObject.tag.Equals("Truck") && truckCount > 0){
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
		truckCount++;

		Vector2 fireHouseTilePos;
		_map.Map.GetFireHouseCoordinates (out fireHouseTilePos);
		
		Vector3 truckPos = _map.GetPositionForTile (Mathf.FloorToInt(fireHouseTilePos.x),
		                                       Mathf.FloorToInt(fireHouseTilePos.y));
		truckPos.x += .5f;
		truckPos.z -= .5f;

		GameObject truck = (GameObject)Instantiate (firetruckPrefab);
		truck.transform.position = truckPos;
		
		EGFiretruck firetruck = truck.GetComponent<EGFiretruck>();
		firetruck.SetPosition (truckPos);
		
		TDPath truckPath = new TDPath ();
		TDMap dataMap = _map.Map;
		firetruck.SetMap (_map);
		truckPath.BuildPath (dataMap,
		                     dataMap.GetTile(Mathf.FloorToInt(fireHouseTilePos.x), Mathf.FloorToInt(fireHouseTilePos.y)),
		                     dataMap.GetTile (x, -z));
		
		firetruck.SetPath (truckPath);
		_dispatcher.AddActiveTruck(firetruck);
	}
}
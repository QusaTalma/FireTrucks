using UnityEngine;
using System.Collections;

public class EGFirehouse : MonoBehaviour {
	int truckCount = 0;

	private EDFirehouse _firehouse;
	TGMap _map;

	public GameObject firetruckPrefab;
	
	void Start()
	{
		_firehouse = new EDFirehouse();

		//set the tile size of the texture (in UV units), based on the rows and columns
		Vector2 size = new Vector2(1f / UnifiedAnimator.FLAME_COLUMNS, 1f / UnifiedAnimator.FLAME_ROWS);
		GetComponent<Renderer>().sharedMaterial.SetTextureScale("_MainTex", size);
	}
	
	void Update(){
		if (truckCount == 0) {
			SpawnNextTruck ();
		}
	}

	public void SetTGMap(TGMap map){
		_map = map;
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
		firetruck.SetMap (_map);

		EGDispatcher.Instance.SendTruckToTile (firetruck, x, -z);
	}
}
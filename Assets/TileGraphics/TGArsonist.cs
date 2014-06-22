using UnityEngine;
using System.Collections.Generic;

public class TGArsonist : MonoBehaviour
{
	TGMap _map;
	TDTile _currentTile;
	TDTile _previousTile;

	int x;
	int y;

	public float fireLightInterval;
	public float moveInterval;

	float timeSinceFireLight = 0f;
	float timeSinceMove = 0f;

	public GameObject flamePrefab;

	public void Update(){
		timeSinceFireLight += Time.deltaTime;
		timeSinceMove += Time.deltaTime;

		if (timeSinceFireLight >= fireLightInterval) {
			StartNearbyHouseOnFire();
			timeSinceFireLight = 0f;
		}

		if (timeSinceMove >= moveInterval) {
			MoveToNearbyTile();
			timeSinceMove = 0f;
		}
	}

	public void SetMap(TGMap map){
		this._map = map;
	}

	public void SetPosition(int x, int y){
		this.x = x;
		this.y = y;

		_currentTile = _map.GetDataMap ().GetTile (x, y);
	}

	void StartNearbyHouseOnFire(){
		if (!HaveNeighboringFires ()) {
			List<TDTile> houses = GetNeighboringHouses();
			if(houses.Count > 0){
				Debug.Log(houses);
				int houseIndexToIgnite = Random.Range(0, houses.Count);
				
				Debug.Log("index to ignite " + houseIndexToIgnite);
				Debug.Log("houses.Count " + houses.Count);
				TDTile tileToIgnite = houses[houseIndexToIgnite];
				GameObject flame = (GameObject)Instantiate (flamePrefab);
				
				Vector3 flamePos = _map.GetPositionForTile (tileToIgnite.GetX(), tileToIgnite.GetY());
				flamePos.x += 0.5f;
				flamePos.z -= 0.5f;
				
				flame.transform.position = flamePos;
				
				tileToIgnite.type = TDTile.TILE_HOUSE_ON_FIRE;
			}
		}
	}

	bool HaveNeighboringFires(){
		List<TDTile> neighboringFires = _map.GetDataMap ().FindAdjacentTilesOfType (_currentTile, TDTile.TILE_HOUSE_ON_FIRE);
		return neighboringFires.Count > 0;
	}

	List<TDTile> GetNeighboringHouses(){
		return _map.GetDataMap ().FindAdjacentTilesOfType (_currentTile, TDTile.TILE_HOUSE);
	}

	void MoveToNearbyTile(){
		List<TDTile> neighboringTiles = _map.GetDataMap ().FindAdjacentTilesOfType (_currentTile, TDTile.TILE_STREET);
		neighboringTiles.Remove (_previousTile);

		int tileIndex = Random.Range (0, neighboringTiles.Count);

		_previousTile = _currentTile;
		_currentTile = neighboringTiles [tileIndex];
	}
}